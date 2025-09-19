using MediatR;

namespace FarmXpert.Application.Todo.Commands.CreateTodo;

public record CreateTodoCommand(string Title, bool IsCompleted): IRequest<Domain.Entities.Todo>;