using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.Vehicle.Queries.GetVehicleById;

public record GetVehicleByIdQuery(string OwnerId, Guid Id) : IRequest<FarmXpert.Domain.Entities.Vehicle?>;
