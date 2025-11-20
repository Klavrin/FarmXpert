using MediatR;

namespace FarmXpert.Application.Animal.Commands.DeleteAnimal;
public record DeleteAnimalCommand(string OwnerId, Guid Id) : IRequest<FarmXpert.Domain.Entities.Animal?>;
