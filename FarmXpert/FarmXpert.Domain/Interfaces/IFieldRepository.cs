using FarmXpert.Domain.Entities;

namespace FarmXpert.Domain.Interfaces;

public interface IFieldRepository
{
    Task CreateAsync(Field field, CancellationToken cancellationToken = default);
    Task<Field?> GetByIdAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Field>> GetAllAsync(string OwnerId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Field field, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
