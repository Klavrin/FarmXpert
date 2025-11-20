using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Animal.Commands.UpdateAnimal;

public class UpdateAnimalCommandHandler : IRequestHandler<UpdateAnimalCommand, Domain.Entities.Animal>
{
    private readonly IAnimalRepository _animalRepository;

    public UpdateAnimalCommandHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<Domain.Entities.Animal> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
    {
        var existingAnimal = await _animalRepository.GetByIdAsync(request.Animal.OwnerId, request.Animal.Id, cancellationToken);
        if (existingAnimal == null)
        {
            throw new KeyNotFoundException($"Animal with ID {request.Animal.Id} not found.");
        }

        existingAnimal.CattleId = request.Animal.CattleId;
        existingAnimal.Species = request.Animal.Species;
        existingAnimal.Sex = request.Animal.Sex;
        existingAnimal.BirthDate = request.Animal.BirthDate;

        await _animalRepository.UpdateAsync(existingAnimal, cancellationToken);
        return existingAnimal;
    }
}
