using MediatR;

namespace FarmXpert.Application.Todo.Commands.DeleteTodo;

public record DeleteTodoCommand(Guid Id): IRequest<Domain.Entities.Todo?>;
