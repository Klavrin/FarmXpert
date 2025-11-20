using MediatR;

namespace FarmXpert.Application.Todo.Commands.UpdateTodo;

public record UpdateTodoCommand(Domain.Entities.Todo Todo) : IRequest<Domain.Entities.Todo?>;
