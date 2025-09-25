using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MongoDB.Driver;

namespace FarmXpert.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly IMongoCollection<Vehicle> _vehicles;

    public VehicleRepository(IMongoDatabase database)
    {
        _vehicles = database.GetCollection<Vehicle>("vehicles");
    }

    public async Task CreateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        await _vehicles.InsertOneAsync(vehicle, null, cancellationToken);
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Vehicle>.Filter.Eq(v => v.Id, id);
        return await _vehicles.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vehicles.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Vehicle>.Filter.Eq(v => v.Id, vehicle.Id);
        await _vehicles.ReplaceOneAsync(filter, vehicle, new ReplaceOptions(), cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Vehicle>.Filter.Eq(v => v.Id, id);
        await _vehicles.DeleteOneAsync(filter, cancellationToken);
    }
}