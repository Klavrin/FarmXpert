using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.Animal.Queries.GetAnimalById;
public record GetAnimalByIdQuery(string OwnerId, Guid Id) : IRequest<FarmXpert.Domain.Entities.Animal?>;
