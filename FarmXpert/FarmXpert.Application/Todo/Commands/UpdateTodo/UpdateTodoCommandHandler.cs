using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Todo.Commands.UpdateTodo;

public class UpdateTodoCommandHandler: IRequestHandler<UpdateTodoCommand, Domain.Entities.Todo>
{
    private readonly ITodoRepository _todoRepository;
    
    public UpdateTodoCommandHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }
    
    public async Task<Domain.Entities.Todo> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var existingTodo = await _todoRepository.GetByIdAsync(request.Todo.Id, cancellationToken);
        if (existingTodo == null)
        {
            throw new KeyNotFoundException($"Todo with ID {request.Todo.Id} not found.");
        }

        existingTodo.Title = request.Todo.Title;
        existingTodo.IsCompleted = request.Todo.IsCompleted;

        await _todoRepository.UpdateAsync(existingTodo, cancellationToken);
        return existingTodo;
    }
}