using MediatR;

namespace FarmXpert.Application.PersonalDocument.Commands.UpdatePersonalDocument;

public record UpdatePersonalDocumentCommand(Guid Id, string OwnerId, string Title) : IRequest<FarmXpert.Domain.Entities.PersonalDocument>;
