using MediatR;
namespace FarmXpert.Application.ApplicationDocument.Queries.GetAllApplicationDocuments;

public record GetAllApplicationDocumentsQuery(string OwnerId) : IRequest<List<FarmXpert.Domain.Entities.ApplicationDocument>>;
