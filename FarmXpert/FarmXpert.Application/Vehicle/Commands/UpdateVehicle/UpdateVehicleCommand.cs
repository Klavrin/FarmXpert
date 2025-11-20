using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.Vehicle.Commands.UpdateVehicle;

public record UpdateVehicleCommand(FarmXpert.Domain.Entities.Vehicle Vehicle) : IRequest<FarmXpert.Domain.Entities.Vehicle?>;
