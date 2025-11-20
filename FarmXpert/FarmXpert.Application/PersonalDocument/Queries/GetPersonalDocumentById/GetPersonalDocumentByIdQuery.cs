using MediatR;

namespace FarmXpert.Application.PersonalDocument.Queries.GetPersonalDocumentById;

public record GetPersonalDocumentByIdQuery(Guid Id, string OwnerId) : IRequest<FarmXpert.Domain.Entities.PersonalDocument?>;
