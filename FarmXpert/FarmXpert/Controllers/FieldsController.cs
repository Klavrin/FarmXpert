using FarmXpert.Application.Field.Commands.CreateField;
using FarmXpert.Application.Field.Queries.GetAllFields;
using FarmXpert.Application.Field.Queries.GetFieldById;
using FarmXpert.Application.Field.Commands.DeleteField;
using FarmXpert.Application.Field.Commands.UpdateField;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/fields")]
public class FieldsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FieldsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllFieldsQuery()));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var field = await _mediator.Send(new GetFieldByIdQuery(id));
        if (field == null)
            return NotFound();
        return Ok(field);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFieldCommand command)
    {
        var field = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = field.Id }, field);
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
        var field = await _mediator.Send(new GetFieldByIdQuery(id));
        if (field == null)
            return NotFound();

        await _mediator.Send(new DeleteFieldCommand(id));
        return Ok(field);
    }
}