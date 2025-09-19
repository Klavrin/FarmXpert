using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Todo.Queries.GetTodoById;

public class GetTodoByIdQueryHandler: IRequestHandler<GetTodoByIdQuery, Domain.Entities.Todo?>
{
    private readonly ITodoRepository _todoRepository;
    
    public GetTodoByIdQueryHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }
    
    public async Task<Domain.Entities.Todo?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        return await _todoRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}