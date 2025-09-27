using MediatR;

namespace FarmXpert.Application.Field.Queries.GetAllFields;
public record GetAllFieldsQuery() : IRequest<List<FarmXpert.Domain.Entities.Field>>;