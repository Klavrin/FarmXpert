using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.Vehicle.Commands.CreateVehicle;

public record CreateVehicleCommand(
    Guid BusinessId,
    Guid VehicleGroupId,
    string VehicleType,
    short FabricationDate,
    string Brand
) : IRequest<FarmXpert.Domain.Entities.Vehicle>;