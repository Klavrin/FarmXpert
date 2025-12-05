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
public class VehiclesController : BaseApiController
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var vehicles = await _mediator.Send(new GetAllVehiclesQuery(userId));
        return Ok(vehicles);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetCurrentUserId();
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
        var userId = GetCurrentUserId();
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
        var userid = GetCurrentUserId();
        var vehicle = await _mediator.Send(new GetVehicleByIdQuery(userid, id));
        if (vehicle == null)
            return NotFound();

        await _mediator.Send(new DeleteVehicleCommand(userid, id));
        return Ok(vehicle);
    }
}
