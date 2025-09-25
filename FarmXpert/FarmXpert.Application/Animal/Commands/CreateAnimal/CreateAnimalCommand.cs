using MediatR;
using FarmXpert.Domain.Enums;
using FarmXpert.Domain.Entities;

namespace FarmXpert.Application.Animal.Commands.CreateAnimal;
public record CreateAnimalCommand(
    Guid CattleId,
    string Species,
    Sex Sex,
    DateTime BirthDate
) : IRequest<FarmXpert.Domain.Entities.Animal>;