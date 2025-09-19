using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Todo.Commands.CreateTodo;

public class CreateTodoCommandHanlder: IRequestHandler<CreateTodoCommand, Domain.Entities.Todo>
{
    private readonly ITodoRepository _todoRepository;

    public CreateTodoCommandHanlder(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<Domain.Entities.Todo> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = new Domain.Entities.Todo
        {
            Title = request.Title,
            IsCompleted = request.IsCompleted
        };
        
        await _todoRepository.CreateAsync(todo, cancellationToken);
        return todo;
    }
}