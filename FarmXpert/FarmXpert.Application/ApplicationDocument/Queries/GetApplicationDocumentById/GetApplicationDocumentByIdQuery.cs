using MediatR;


namespace FarmXpert.Application.ApplicationDocument.Queries.GetApplicationDocumentById;

public record GetApplicationDocumentByIdQuery(Guid Id, string OwnerId) : IRequest<FarmXpert.Domain.Entities.ApplicationDocument?>;
