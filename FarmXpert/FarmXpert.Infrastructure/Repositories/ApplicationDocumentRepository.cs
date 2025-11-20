using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MongoDB.Driver;

namespace FarmXpert.Infrastructure.Repositories;

public class ApplicationDocumentRepository : IApplicationDocumentRepository
{
    private readonly IMongoCollection<ApplicationDocument> _applicationDocument;

    public ApplicationDocumentRepository(IMongoDatabase database)
    {
        _applicationDocument = database.GetCollection<ApplicationDocument>("applicationDocuments");
    }

    public async Task CreateAsync(ApplicationDocument document, CancellationToken cancellationToken = default)
        => await _applicationDocument.InsertOneAsync(document, null, cancellationToken);

    public async Task<IEnumerable<ApplicationDocument>> GetAllAsync(string OwnerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ApplicationDocument>.Filter.Eq(a => a.OwnerId, OwnerId);
        return await _applicationDocument.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<ApplicationDocument?> GetByIdAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ApplicationDocument>.Filter.Eq(a => a.OwnerId, OwnerId) & Builders<ApplicationDocument>.Filter.Eq(a => a.Id, id);
        return await _applicationDocument.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(ApplicationDocument document, CancellationToken cancellationToken = default)
        => await _applicationDocument.ReplaceOneAsync(Builders<ApplicationDocument>.Filter.Eq(f => f.Id, document.Id), document, new ReplaceOptions(), cancellationToken);

    public async Task DeleteAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ApplicationDocument>.Filter.Eq(a => a.OwnerId, OwnerId) & Builders<ApplicationDocument>.Filter.Eq(a => a.Id, id);
        await _applicationDocument.DeleteOneAsync(filter, cancellationToken);
    }
}
