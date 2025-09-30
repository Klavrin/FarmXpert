using FarmXpert.Application.Animal.Commands.CreateAnimal;
using FarmXpert.Application.Animal.Commands.DeleteAnimal;   
using FarmXpert.Application.Animal.Commands.UpdateAnimal;
using FarmXpert.Application.Animal.Queries.GetAllAnimals;
using FarmXpert.Application.Animal.Queries.GetAnimalById;
using FarmXpert.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/animals")]
[Authorize]
public class AnimalsController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMediator _mediator;

    public AnimalsController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = CurrentUserId();
        var animals = await _mediator.Send(new GetAllAnimalsQuery(userId));
        return Ok(animals);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = CurrentUserId();
        var animal = await _mediator.Send(new GetAnimalByIdQuery(userId,id));
        if (animal == null)
        {
            return NotFound();
        }
        return Ok(animal);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Animal animal)
    {
        var userId = CurrentUserId();
        var command = new CreateAnimalCommand(
            OwnerId: userId,
            CattleId: animal.CattleId,
            Species: animal.Species,
            Sex: animal.Sex,
            BirthDate: animal.BirthDate
        );

        var created = await _mediator.Send(command);
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
        var userId = CurrentUserId();
        var animal = await _mediator.Send(new GetAnimalByIdQuery(userId,id));
        if (animal == null)
        {
            return NotFound();
        }

        await _mediator.Send(new DeleteAnimalCommand(userId, id));
        return Ok(animal);
    }

    private string CurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not authenticated");
    }
}