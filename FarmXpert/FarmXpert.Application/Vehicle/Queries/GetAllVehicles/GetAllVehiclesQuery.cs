using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.Vehicle.Queries.GetAllVehicles;

public record GetAllVehiclesQuery(string OwnerId) : IRequest<List<FarmXpert.Domain.Entities.Vehicle>>;