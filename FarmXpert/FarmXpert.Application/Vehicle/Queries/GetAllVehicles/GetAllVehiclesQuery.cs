using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.Vehicle.Queries.GetAllVehicles;

public record GetAllVehiclesQuery() : IRequest<List<FarmXpert.Domain.Entities.Vehicle>>;