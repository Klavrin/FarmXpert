using FarmXpert.Application.Vehicle.Commands.CreateVehicle;
using FarmXpert.Application.Vehicle.Commands.DeleteVehicle;
using FarmXpert.Application.Vehicle.Commands.UpdateVehicle;
using FarmXpert.Application.Vehicle.Queries.GetAllVehicles;
using FarmXpert.Application.Vehicle.Queries.GetVehicleById;
using FarmXpert.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vehicles = await _mediator.Send(new GetAllVehiclesQuery());
        return Ok(vehicles);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var vehicle = await _mediator.Send(new GetVehicleByIdQuery(id));
        if (vehicle == null)
        {
            return NotFound();
        }
        return Ok(vehicle);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleCommand command)
    {
        try
        {
            var vehicle = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(UpdateVehicleCommand command)
    {
        try
        {
            var updatedVehicle = await _mediator.Send(command);
            if (updatedVehicle == null)
            {
                return BadRequest("Failed to update vehicle.");
            }
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
        var vehicle = await _mediator.Send(new GetVehicleByIdQuery(id));
        if (vehicle == null)
        {
            return NotFound();
        }

        await _mediator.Send(new DeleteVehicleCommand(id));
        return Ok(vehicle);
    }
}