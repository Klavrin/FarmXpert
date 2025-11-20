using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Animal.Queries.GetAllAnimals;
public class GetAllAnimalsQueryHandler : IRequestHandler<GetAllAnimalsQuery, List<FarmXpert.Domain.Entities.Animal>>
{
    private readonly IAnimalRepository _animalRepository;

    public GetAllAnimalsQueryHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<List<FarmXpert.Domain.Entities.Animal>> Handle(GetAllAnimalsQuery request, CancellationToken cancellationToken)
    {
        var animals = await _animalRepository.GetAllAsync(request.OwnerId, cancellationToken);
        return animals.ToList();
    }
}
