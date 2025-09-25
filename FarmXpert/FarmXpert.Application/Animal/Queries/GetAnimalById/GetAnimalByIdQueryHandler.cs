using FarmXpert.Domain.Interfaces;
using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.Animal.Queries.GetAnimalById;
public class GetAnimalByIdQueryHandler : IRequestHandler<GetAnimalByIdQuery, FarmXpert.Domain.Entities.Animal?>
{
    private readonly IAnimalRepository _animalRepository;

    public GetAnimalByIdQueryHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<Domain.Entities.Animal?> Handle(GetAnimalByIdQuery request, CancellationToken cancellationToken)
    {
        return await _animalRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}