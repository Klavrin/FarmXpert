using MediatR;

namespace FarmXpert.Application.ApplicationDocument.Commands.DeleteApplicationDocument;

public record DeleteApplicationDocumentCommand() : IRequest<FarmXpert.Domain.Entities.ApplicationDocument>;
