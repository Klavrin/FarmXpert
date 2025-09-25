using FarmXpert.Application.Animal.Commands.CreateAnimal;
using FarmXpert.Application.Animal.Queries.GetAllAnimals;
using FarmXpert.Application.Animal.Queries.GetAnimalById;
using FarmXpert.Application.Animal.Commands.DeleteAnimal;   
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FarmXpert.Application.Animal.Commands.UpdateAnimal;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/animals")]
public class AnimalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnimalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllAnimalsQuery()));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var animal = await _mediator.Send(new GetAnimalByIdQuery(id));
        if (animal == null)
        {
            return NotFound();
        }
        return Ok(animal);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnimalCommand command)
    {
        var animal = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = animal.Id }, animal);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(UpdateAnimalCommand command)
    {
        try
        {
            var updatedAnimal = await _mediator.Send(command);
            if (updatedAnimal == null)
            {
                return BadRequest("Failed to modify animal information.");
            }

            return Ok(updatedAnimal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var animal = await _mediator.Send(new GetAnimalByIdQuery(id));
        if (animal == null)
        {
            return NotFound();
        }

        await _mediator.Send(new DeleteAnimalCommand(id));
        return Ok(animal);
    }
}