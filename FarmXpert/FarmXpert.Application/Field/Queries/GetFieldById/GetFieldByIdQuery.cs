using MediatR;

namespace FarmXpert.Application.Field.Queries.GetFieldById;
public record GetFieldByIdQuery(Guid Id) : IRequest<FarmXpert.Domain.Entities.Field?>;