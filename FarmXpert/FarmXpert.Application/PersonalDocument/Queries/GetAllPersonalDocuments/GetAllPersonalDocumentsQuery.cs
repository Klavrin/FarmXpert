using MediatR;

namespace FarmXpert.Application.PersonalDocument.Queries.GetAllPersonalDocuments;

public record GetAllPersonalDocumentsQuery(string OwnerId) : IRequest<List<FarmXpert.Domain.Entities.PersonalDocument>>;
