using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MongoDB.Driver;

namespace FarmXpert.Infrastructure.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly IMongoCollection<Animal> _animals;

    public AnimalRepository(IMongoDatabase database)
    {
        _animals = database.GetCollection<Animal>("animals");
    }

    public async Task CreateAsync(Animal animal, CancellationToken cancellationToken = default)
        => await _animals.InsertOneAsync(animal, null, cancellationToken);

    public async Task<Animal?> GetByIdAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Animal>.Filter.Eq(a=>a.OwnerId, OwnerId) & Builders<Animal>.Filter.Eq(a => a.Id, id);
        return await _animals.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Animal>> GetAllAsync(string OwnerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Animal>.Filter.Eq(a => a.OwnerId, OwnerId);
        return await _animals.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Animal animal, CancellationToken cancellationToken = default)
        => await _animals.ReplaceOneAsync(Builders<Animal>.Filter.Eq(a => a.Id, animal.Id), animal, new ReplaceOptions(), cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => await _animals.DeleteOneAsync(Builders<Animal>.Filter.Eq(a => a.Id, id), cancellationToken);
}