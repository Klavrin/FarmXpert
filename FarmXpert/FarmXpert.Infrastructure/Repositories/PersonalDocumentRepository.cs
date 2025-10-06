using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmXpert.Infrastructure.Repositories;
public class PersonalDocumentRepository : IPersonalDocumentRepository
{
    private readonly IMongoCollection<PersonalDocument> _personaldocuments;

    public PersonalDocumentRepository(IMongoDatabase database)
    {
        _personaldocuments = database.GetCollection<PersonalDocument>("personaldocuments");
    }

    public async Task CreateAsync(PersonalDocument personaldocument, CancellationToken cancellationToken = default)
        => await _personaldocuments.InsertOneAsync(personaldocument, null, cancellationToken);

    public async Task<IEnumerable<PersonalDocument>> GetAllAsync(string OwnerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<PersonalDocument>.Filter.Eq(a => a.OwnerId, OwnerId);
        return await _personaldocuments.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<PersonalDocument?> GetByIdAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<PersonalDocument>.Filter.Eq(a => a.OwnerId, OwnerId) & Builders<PersonalDocument>.Filter.Eq(a => a.Id, id);
        return await _personaldocuments.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(PersonalDocument personaldocument, CancellationToken cancellationToken = default)
    {
        var filter = Builders<PersonalDocument>.Filter.Eq(a => a.OwnerId, personaldocument.OwnerId) & Builders<PersonalDocument>.Filter.Eq(a => a.Id, personaldocument.Id);
        await _personaldocuments.ReplaceOneAsync(filter, personaldocument, new ReplaceOptions(), cancellationToken);
    }

    public async Task DeleteAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<PersonalDocument>.Filter.Eq(a => a.OwnerId, OwnerId) & Builders<PersonalDocument>.Filter.Eq(a => a.Id, id);
        await _personaldocuments.DeleteOneAsync(filter, cancellationToken);
    }
}