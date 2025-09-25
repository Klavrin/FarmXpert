using MediatR;

namespace FarmXpert.Application.Animal.Commands.DeleteAnimal;
public record DeleteAnimalCommand(Guid Id) : IRequest<FarmXpert.Domain.Entities.Animal?>;
