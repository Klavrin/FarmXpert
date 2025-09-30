using FarmXpert.Domain.Entities;

namespace FarmXpert.Domain.Interfaces;

public interface IVehicleRepository
{
    Task CreateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByIdAsync(string ownerId, Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> GetAllAsync(string ownerId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}