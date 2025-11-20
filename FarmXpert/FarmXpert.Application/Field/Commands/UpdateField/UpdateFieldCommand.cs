using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.Field.Commands.UpdateField;
public record UpdateFieldCommand(FarmXpert.Domain.Entities.Field Field) : IRequest<FarmXpert.Domain.Entities.Field?>;
