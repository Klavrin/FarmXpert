using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Animal.Commands.DeleteAnimal;

public class DeleteAnimalCommandHandler : IRequestHandler<DeleteAnimalCommand, FarmXpert.Domain.Entities.Animal?>
{
    private readonly IAnimalRepository _animalRepository;

    public DeleteAnimalCommandHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<Domain.Entities.Animal?> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = await _animalRepository.GetByIdAsync(request.OwnerId, request.Id, cancellationToken);
        if (animal == null)
        {
            return null;
        }

        await _animalRepository.DeleteAsync(request.Id, cancellationToken);
        return animal;
    }
}
