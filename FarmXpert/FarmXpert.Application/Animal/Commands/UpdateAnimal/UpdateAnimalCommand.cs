using MediatR;

namespace FarmXpert.Application.Animal.Commands.UpdateAnimal;

public record UpdateAnimalCommand(Domain.Entities.Animal Animal) : IRequest<Domain.Entities.Animal?>;

