using FarmXpert.Application.Animal.Commands.CreateAnimal;
using FarmXpert.Application.Animal.Commands.DeleteAnimal;
using FarmXpert.Application.Animal.Commands.UpdateAnimal;
using FarmXpert.Application.Animal.Queries.GetAllAnimals;
using FarmXpert.Application.Animal.Queries.GetAnimalById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/animals")]
[Authorize]
public class AnimalsController : BaseApiController
{

    private readonly IMediator _mediator;

    public AnimalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var animals = await _mediator.Send(new GetAllAnimalsQuery(userId));
        return Ok(animals);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetCurrentUserId();
        var animal = await _mediator.Send(new GetAnimalByIdQuery(userId, id));
        if (animal == null)
        {
            return NotFound();
        }
        return Ok(animal);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAnimalCommand command)
    {
        var userId = GetCurrentUserId();
        var commandWithUserId = command with { OwnerId = userId };
        var created = await _mediator.Send(commandWithUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
        var userId = GetCurrentUserId();
        var animal = await _mediator.Send(new GetAnimalByIdQuery(userId, id));
        if (animal == null)
        {
            return NotFound();
        }

        await _mediator.Send(new DeleteAnimalCommand(userId, id));
        return Ok(animal);
    }
}
