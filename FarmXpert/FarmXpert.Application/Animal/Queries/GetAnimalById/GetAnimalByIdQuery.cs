using MediatR;
using FarmXpert.Domain.Entities;

namespace FarmXpert.Application.Animal.Queries.GetAnimalById;
public record GetAnimalByIdQuery(Guid Id) : IRequest<FarmXpert.Domain.Entities.Animal?>;
