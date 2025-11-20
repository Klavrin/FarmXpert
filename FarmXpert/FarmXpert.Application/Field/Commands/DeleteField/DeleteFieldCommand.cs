using MediatR;

namespace FarmXpert.Application.Field.Commands.DeleteField;
public record DeleteFieldCommand(string OwnerId, Guid Id) : IRequest<FarmXpert.Domain.Entities.Field?>;
