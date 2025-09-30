using MediatR;

namespace FarmXpert.Application.Field.Queries.GetFieldById;
public record GetFieldByIdQuery(string OwnerId, Guid Id) : IRequest<FarmXpert.Domain.Entities.Field?>;