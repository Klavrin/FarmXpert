using MediatR;

namespace FarmXpert.Application.PersonalDocument.Commands.CreatePersonalDocument;
public record CreatePersonalDocumentCommand(string Title, Stream FileStream, string FileExtension, string OwnerId) : IRequest<FarmXpert.Domain.Entities.PersonalDocument>;
