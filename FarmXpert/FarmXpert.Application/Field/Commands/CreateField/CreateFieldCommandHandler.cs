using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Field.Commands.CreateField;
public class CreateFieldCommandHandler : IRequestHandler<CreateFieldCommand, FarmXpert.Domain.Entities.Field>
{
    private readonly IFieldRepository _fieldRepository;

    public CreateFieldCommandHandler(IFieldRepository fieldRepository)
    {
        _fieldRepository = fieldRepository;
    }

    public async Task<FarmXpert.Domain.Entities.Field> Handle(CreateFieldCommand request, CancellationToken cancellationToken)
    {
        var field = new FarmXpert.Domain.Entities.Field
        {
            Id = Guid.NewGuid(),
            BusinessId = request.BusinessId,
            CropType = request.CropType,
            OtherCropType = request.OtherCropType,
            SoilType = request.SoilType,
            OtherSoilType = request.OtherSoilType,
            Fertilizer = request.Fertilizer,
            OtherFertilizer = request.OtherFertilizer,
            Herbicide = request.Herbicide,
            OtherHerbicide = request.OtherHerbicide,
            Coords = request.Coords,
            OwnerId = request.OwnerId

        };
        await _fieldRepository.CreateAsync(field, cancellationToken);
        return field;
    }
}