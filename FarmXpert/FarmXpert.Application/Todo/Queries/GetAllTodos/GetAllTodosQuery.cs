using MediatR;

namespace FarmXpert.Application.Todo.Queries.GetAllTodos;

public record GetAllTodosQuery : IRequest<List<Domain.Entities.Todo>>;
