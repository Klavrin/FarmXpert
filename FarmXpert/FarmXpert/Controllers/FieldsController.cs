using FarmXpert.Application.Field.Commands.CreateField;
using FarmXpert.Application.Field.Commands.DeleteField;
using FarmXpert.Application.Field.Commands.UpdateField;
using FarmXpert.Application.Field.Queries.GetAllFields;
using FarmXpert.Application.Field.Queries.GetFieldById;
using FarmXpert.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FarmXpert.Controllers;

[ApiController]
[Route("api/fields")]
[Authorize]
public class FieldsController : BaseApiController
{
    private readonly IMediator _mediator;

    public FieldsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all fields for the current user.
    /// </summary>
    /// <returns>A list of all fields owned by the authenticated user.</returns>
    /// <response code="200">Returns the list of fields.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var fields = await _mediator.Send(new GetAllFieldsQuery(userId));
        return Ok(fields);
    }

    /// <summary>
    /// Retrieves a specific field by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the field.</param>
    /// <returns>The field details if found.</returns>
    /// <response code="200">Returns the field details.</response>
    /// <response code="404">If the field is not found or does not belong to the user.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetCurrentUserId();
        var field = await _mediator.Send(new GetFieldByIdQuery(userId, id));
        if (field == null)
        {
            return NotFound();
        }
        return Ok(field);
    }

    /// <summary>
    /// Creates a new field for the current user.
    /// </summary>
    /// <param name="field">The field entity containing crop type, soil type, fertilizer, herbicide, and coordinate information.</param>
    /// <returns>The newly created field.</returns>
    /// <response code="201">Returns the newly created field.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Field field)
    {
        var userId = GetCurrentUserId();
        var command = new CreateFieldCommand
        (
            BusinessId: field.BusinessId,
            CropType: field.CropType,
            OtherCropType: field.OtherCropType,
            SoilType: field.SoilType,
            OtherSoilType: field.OtherSoilType,
            Fertilizer: field.Fertilizer,
            OtherFertilizer: field.OtherFertilizer,
            Herbicide: field.Herbicide,
            OtherHerbicide: field.OtherHerbicide,
            Coords: field.Coords ?? new List<double[]>(),
            OwnerId: userId
        );
        var createdField = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = createdField.Id }, createdField);
    }

    /// <summary>
    /// Updates an existing field's information.
    /// </summary>
    /// <param name="command">The command containing updated field details.</param>
    /// <returns>The updated field details.</returns>
    /// <response code="200">Returns the updated field.</response>
    /// <response code="400">If the update fails or the request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(UpdateFieldCommand command)
    {
        try
        {
            var updatedField = await _mediator.Send(command);
            if (updatedField == null)
                return BadRequest("Failed to modify field information.");
            return Ok(updatedField);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a specific field by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the field to delete.</param>
    /// <returns>The deleted field details.</returns>
    /// <response code="200">Returns the deleted field details.</response>
    /// <response code="404">If the field is not found or does not belong to the user.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userid = GetCurrentUserId();
        var field = await _mediator.Send(new GetFieldByIdQuery(userid, id));
        if (field == null)
            return NotFound();
        await _mediator.Send(new DeleteFieldCommand(userid, id));
        return Ok(field);
    }
}
