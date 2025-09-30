using FarmXpert.Application.Field.Commands.DeleteField;
using FarmXpert.Application.Field.Commands.UpdateField;
using FarmXpert.Application.Field.Queries.GetFieldById;
using FarmXpert.Application.Vehicle.Commands.CreateVehicle;
using FarmXpert.Application.Vehicle.Commands.DeleteVehicle;
using FarmXpert.Application.Vehicle.Commands.UpdateVehicle;
using FarmXpert.Application.Vehicle.Queries.GetAllVehicles;
using FarmXpert.Application.Vehicle.Queries.GetVehicleById;
using FarmXpert.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/vehicles")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VehiclesController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = CurrentUserId();
        var vehicles = await _mediator.Send(new GetAllVehiclesQuery(userId));
        return Ok(vehicles);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = CurrentUserId();
        var vehicle = await _mediator.Send(new GetVehicleByIdQuery(userId, id));
        if (vehicle == null)
        {
            return NotFound();
        }
        return Ok(vehicle);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Vehicle vehicle)
    {
        var userId = CurrentUserId();
        var command = new CreateVehicleCommand(
            OwnerId: userId,
            BusinessId: vehicle.BusinessId,
            VehicleGroupId: vehicle.VehicleGroupId,
            VehicleType: vehicle.VehicleType,
            FabricationDate: vehicle.FabricationDate,
            Brand: vehicle.Brand
        );

        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(UpdateVehicleCommand command)
    {
        try
        {
            var updatedVehicle = await _mediator.Send(command);
            if (updatedVehicle == null)
                return BadRequest("Failed to modify field information.");
            return Ok(updatedVehicle);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userid = CurrentUserId();
        var vehicle = await _mediator.Send(new GetVehicleByIdQuery(userid, id));
        if (vehicle == null)
            return NotFound();

        await _mediator.Send(new DeleteVehicleCommand(userid, id));
        return Ok(vehicle);
    }

    private string CurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not authenticated");
    }
}