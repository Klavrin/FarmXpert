using MediatR;

namespace FarmXpert.Application.Field.Commands.DeleteField;
public record DeleteFieldCommand(Guid Id) : IRequest<FarmXpert.Domain.Entities.Field?>;