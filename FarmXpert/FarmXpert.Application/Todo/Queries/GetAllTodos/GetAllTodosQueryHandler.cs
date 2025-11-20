using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Todo.Queries.GetAllTodos;

public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, List<Domain.Entities.Todo>>
{
    private readonly ITodoRepository _todoRepository;

    public GetAllTodosQueryHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<List<Domain.Entities.Todo>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
    {
        var todos = await _todoRepository.GetAllAsync(cancellationToken);
        return todos.ToList();
    }
}
