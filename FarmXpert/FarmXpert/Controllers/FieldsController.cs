
using FarmXpert.Application.Field.Commands.CreateField;
using FarmXpert.Application.Field.Commands.DeleteField;
using FarmXpert.Application.Field.Commands.UpdateField;
using FarmXpert.Application.Field.Queries.GetAllFields;
using FarmXpert.Application.Field.Queries.GetFieldById;
using FarmXpert.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var fields = await _mediator.Send(new GetAllFieldsQuery(userId));
        return Ok(fields);
    }

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
