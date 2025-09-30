using MediatR;

namespace FarmXpert.Application.Vehicle.Commands.DeleteVehicle;

public record DeleteVehicleCommand(string OwnerId, Guid Id) : IRequest<FarmXpert.Domain.Entities.Vehicle?>;