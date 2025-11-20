using MediatR;

namespace FarmXpert.Application.Field.Queries.GetAllFields;
public record GetAllFieldsQuery(string OwnerId) : IRequest<List<FarmXpert.Domain.Entities.Field>>;
