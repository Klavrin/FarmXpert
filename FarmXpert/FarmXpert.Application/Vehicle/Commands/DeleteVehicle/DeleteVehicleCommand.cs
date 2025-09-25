using MediatR;

namespace FarmXpert.Application.Vehicle.Commands.DeleteVehicle;

public record DeleteVehicleCommand(Guid Id) : IRequest<FarmXpert.Domain.Entities.Vehicle?>;