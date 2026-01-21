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

    /// <summary>
    /// Retrieves all vehicles for the current user.
    /// </summary>
    /// <returns>A list of all vehicles owned by the authenticated user.</returns>
    /// <response code="200">Returns the list of vehicles.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var vehicles = await _mediator.Send(new GetAllVehiclesQuery(userId));
        return Ok(vehicles);
    }

    /// <summary>
    /// Retrieves a specific vehicle by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle.</param>
    /// <returns>The vehicle details if found.</returns>
    /// <response code="200">Returns the vehicle details.</response>
    /// <response code="404">If the vehicle is not found or does not belong to the user.</response>
    /// <response code="401">If the user is not authenticated.</response>
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

    /// <summary>
    /// Creates a new vehicle for the current user.
    /// </summary>
    /// <param name="vehicle">The vehicle entity containing type, fabrication date, brand, and group information.</param>
    /// <returns>The newly created vehicle.</returns>
    /// <response code="201">Returns the newly created vehicle.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
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

    /// <summary>
    /// Updates an existing vehicle's information.
    /// </summary>
    /// <param name="command">The command containing updated vehicle details.</param>
    /// <returns>The updated vehicle details.</returns>
    /// <response code="200">Returns the updated vehicle.</response>
    /// <response code="400">If the update fails or the request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
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

    /// <summary>
    /// Deletes a specific vehicle by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle to delete.</param>
    /// <returns>The deleted vehicle details.</returns>
    /// <response code="200">Returns the deleted vehicle details.</response>
    /// <response code="404">If the vehicle is not found or does not belong to the user.</response>
    /// <response code="401">If the user is not authenticated.</response>
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
