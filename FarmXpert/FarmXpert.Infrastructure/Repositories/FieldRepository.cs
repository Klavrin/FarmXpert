using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MongoDB.Driver;

namespace FarmXpert.Infrastructure.Repositories;

public class FieldRepository : IFieldRepository
{
    private readonly IMongoCollection<Field> _fields;

    public FieldRepository(IMongoDatabase database)
    {
        _fields = database.GetCollection<Field>("fields");
    }

    public async Task CreateAsync(Field field, CancellationToken cancellationToken = default)
        => await _fields.InsertOneAsync(field, null, cancellationToken);

    public async Task<Field?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _fields.Find(Builders<Field>.Filter.Eq(f => f.Id, id)).FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<Field>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _fields.Find(_ => true).ToListAsync(cancellationToken);

    public async Task UpdateAsync(Field field, CancellationToken cancellationToken = default)
        => await _fields.ReplaceOneAsync(Builders<Field>.Filter.Eq(f => f.Id, field.Id), field, new ReplaceOptions(), cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => await _fields.DeleteOneAsync(Builders<Field>.Filter.Eq(f => f.Id, id), cancellationToken);
}