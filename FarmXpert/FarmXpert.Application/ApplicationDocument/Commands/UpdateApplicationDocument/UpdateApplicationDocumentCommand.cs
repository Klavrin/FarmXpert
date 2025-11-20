using MediatR;

namespace FarmXpert.Application.ApplicationDocument.Commands.UpdateApplicationDocument;

public record UpdateApplicationDocumentCommand(Guid Id, string OwnerId, string Title, string Status, string ReasonRejection) : IRequest<FarmXpert.Domain.Entities.ApplicationDocument?>;
