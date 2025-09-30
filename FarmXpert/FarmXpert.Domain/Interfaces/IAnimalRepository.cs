using FarmXpert.Domain.Entities;

namespace FarmXpert.Domain.Interfaces;

public interface IAnimalRepository
{
    Task CreateAsync(Animal animal, CancellationToken cancellationToken = default);
    Task<Animal?> GetByIdAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Animal>> GetAllAsync(string OwnerId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Animal animal, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
