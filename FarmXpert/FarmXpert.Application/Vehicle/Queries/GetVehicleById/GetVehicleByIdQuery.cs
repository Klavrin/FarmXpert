using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.Vehicle.Queries.GetVehicleById;

public record GetVehicleByIdQuery(Guid Id) : IRequest<FarmXpert.Domain.Entities.Vehicle?>;