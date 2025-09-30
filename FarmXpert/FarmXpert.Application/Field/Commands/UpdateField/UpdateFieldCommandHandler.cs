using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Field.Commands.UpdateField;
public class UpdateFieldCommandHandler : IRequestHandler<UpdateFieldCommand, FarmXpert.Domain.Entities.Field>
{
    private readonly IFieldRepository _fieldRepository;

    public UpdateFieldCommandHandler(IFieldRepository fieldRepository)
    {
        _fieldRepository = fieldRepository;
    }

    public async Task<FarmXpert.Domain.Entities.Field> Handle(UpdateFieldCommand request, CancellationToken cancellationToken)
    {
        var existingField = await _fieldRepository.GetByIdAsync(request.Field.OwnerId, request.Field.Id, cancellationToken);
        if (existingField == null)
            throw new KeyNotFoundException($"Field with ID {request.Field.Id} not found.");

        existingField.BusinessId = request.Field.BusinessId;
        existingField.CropType = request.Field.CropType;
        existingField.OtherCropType = request.Field.OtherCropType;
        existingField.SoilType = request.Field.SoilType;
        existingField.OtherSoilType = request.Field.OtherSoilType;
        existingField.Fertilizer = request.Field.Fertilizer;
        existingField.OtherFertilizer = request.Field.OtherFertilizer;
        existingField.Herbicide = request.Field.Herbicide;
        existingField.OtherHerbicide = request.Field.OtherHerbicide;
        existingField.Coords = request.Field.Coords;

        await _fieldRepository.UpdateAsync(existingField, cancellationToken);
        return existingField;
    }
}