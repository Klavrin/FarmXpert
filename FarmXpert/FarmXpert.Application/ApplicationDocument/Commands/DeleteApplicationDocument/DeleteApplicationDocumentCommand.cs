using MediatR;

namespace FarmXpert.Application.ApplicationDocument.Commands.DeleteApplicationDocument;

public record DeleteApplicationDocumentCommand(Guid id, string OwnerId) : IRequest<FarmXpert.Domain.Entities.ApplicationDocument>;
