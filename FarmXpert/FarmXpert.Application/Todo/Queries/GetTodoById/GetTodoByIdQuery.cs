using MediatR;

namespace FarmXpert.Application.Todo.Queries.GetTodoById;

public record GetTodoByIdQuery(Guid Id) : IRequest<Domain.Entities.Todo?>;
