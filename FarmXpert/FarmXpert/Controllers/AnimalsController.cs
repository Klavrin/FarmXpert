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

    /// <summary>
    /// Retrieves all animals for the current user.
    /// </summary>
    /// <returns>A list of all animals owned by the authenticated user.</returns>
    /// <response code="200">Returns the list of animals.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var animals = await _mediator.Send(new GetAllAnimalsQuery(userId));
        return Ok(animals);
    }

    /// <summary>
    /// Retrieves a specific animal by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the animal.</param>
    /// <returns>The animal details if found.</returns>
    /// <response code="200">Returns the animal details.</response>
    /// <response code="404">If the animal is not found or does not belong to the user.</response>
    /// <response code="401">If the user is not authenticated.</response>
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

    /// <summary>
    /// Creates a new animal for the current user.
    /// </summary>
    /// <param name="command">The command containing animal details to create.</param>
    /// <returns>The newly created animal.</returns>
    /// <response code="201">Returns the newly created animal.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPost]
    public async Task<IActionResult> Create(CreateAnimalCommand command)
    {
        var userId = GetCurrentUserId();
        var commandWithUserId = command with { OwnerId = userId };
        var created = await _mediator.Send(commandWithUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing animal's information.
    /// </summary>
    /// <param name="command">The command containing updated animal details.</param>
    /// <returns>The updated animal details.</returns>
    /// <response code="200">Returns the updated animal.</response>
    /// <response code="400">If the update fails or the request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
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

    /// <summary>
    /// Deletes a specific animal by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the animal to delete.</param>
    /// <returns>The deleted animal details.</returns>
    /// <response code="200">Returns the deleted animal details.</response>
    /// <response code="404">If the animal is not found or does not belong to the user.</response>
    /// <response code="401">If the user is not authenticated.</response>
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
