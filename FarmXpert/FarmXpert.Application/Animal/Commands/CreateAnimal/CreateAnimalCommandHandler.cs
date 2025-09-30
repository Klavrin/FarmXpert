using FarmXpert.Domain.Interfaces;
using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.Animal.Commands.CreateAnimal;
public class CreateAnimalCommandHandler : IRequestHandler<CreateAnimalCommand, FarmXpert.Domain.Entities.Animal>
{
    private readonly IAnimalRepository _animalRepository;

    public CreateAnimalCommandHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<FarmXpert.Domain.Entities.Animal> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = new FarmXpert.Domain.Entities.Animal
        {
            Id = Guid.NewGuid(),
            CattleId = request.CattleId,
            Species = request.Species,
            Sex = request.Sex,
            BirthDate = request.BirthDate,
            OwnerId = request.OwnerId
        };
        await _animalRepository.CreateAsync(animal, cancellationToken);
        return animal;
    }
}