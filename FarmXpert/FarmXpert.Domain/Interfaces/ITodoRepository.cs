using FarmXpert.Domain.Entities;

namespace FarmXpert.Domain.Interfaces;

public interface ITodoRepository
{
    Task CreateAsync(Todo todo, CancellationToken cancellationToken = default);
    Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Todo todo, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
