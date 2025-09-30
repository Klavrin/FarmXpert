using MediatR;
using FarmXpert.Domain.Entities;

namespace FarmXpert.Application.Animal.Queries.GetAllAnimals;

public record GetAllAnimalsQuery(string OwnerId) : IRequest<List<FarmXpert.Domain.Entities.Animal>>;