using MediatR;

namespace FarmXpert.Application.PersonalDocument.Commands.DeletePersonalDocument;

public record DeletePersonalDocumentCommand(Guid Id, string OwnerId) : IRequest<FarmXpert.Domain.Entities.PersonalDocument>;