using FarmXpert.Domain.Enums;
using MediatR;

namespace FarmXpert.Application.Field.Commands.CreateField;
public record CreateFieldCommand(
    Guid BusinessId,
    CropType CropType,
    string OtherCropType,
    SoilType SoilType,
    string OtherSoilType,
    FertilizerType Fertilizer,
    string OtherFertilizer,
    HerbicideType Herbicide,
    string OtherHerbicide,
    List<double[]> Coords,
    string OwnerId
) : IRequest<FarmXpert.Domain.Entities.Field>;
