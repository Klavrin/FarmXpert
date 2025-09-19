using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Todo.Commands.DeleteTodo;

public class DeleteTodoCommandHandler: IRequestHandler<DeleteTodoCommand, Domain.Entities.Todo?>
{
    private readonly ITodoRepository _todoRepository;
    
    public DeleteTodoCommandHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }
    
    public async Task<Domain.Entities.Todo?> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = await _todoRepository.GetByIdAsync(request.Id, cancellationToken);
        if (todo == null)
        {
            return null;
        }
        
        await _todoRepository.DeleteAsync(request.Id, cancellationToken);
        return todo;
    }
}