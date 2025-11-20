using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Enums;
using MediatR;

namespace FarmXpert.Application.Animal.Commands.CreateAnimal;
public record CreateAnimalCommand(
    Guid CattleId,
    string Species,
    Sex Sex,
    DateTime BirthDate,
    string OwnerId
) : IRequest<FarmXpert.Domain.Entities.Animal>;
