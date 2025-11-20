using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MongoDB.Driver;

namespace FarmXpert.Infrastructure.Repositories;

public class SocialPostRepository : ISocialPostRepository
{
    private readonly IMongoCollection<SocialPost> _socialPosts;

    public SocialPostRepository(IMongoDatabase database)
    {
        _socialPosts = database.GetCollection<SocialPost>("socialPosts");
    }
    public async Task CreateAsync(Domain.Entities.SocialPost post, CancellationToken cancellationToken = default)
        => await _socialPosts.InsertOneAsync(post, null, cancellationToken);

    public async Task<IEnumerable<Domain.Entities.SocialPost>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _socialPosts.Find(_ => true).ToListAsync(cancellationToken);
    }
    public async Task<Domain.Entities.SocialPost?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<SocialPost>.Filter.Eq(p => p.Id, id);
        return await _socialPosts.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<IEnumerable<Domain.Entities.SocialPost>> GetAllByUserAsync(string BusinessId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<SocialPost>.Filter.Eq(p => p.BusinessId, BusinessId);
        return await _socialPosts.Find(filter).ToListAsync(cancellationToken);
    }
    public async Task UpdateAsync(Domain.Entities.SocialPost post, CancellationToken cancellationToken = default)
    {
        var filter = Builders<SocialPost>.Filter.Eq(p => p.Id, post.Id);
        await _socialPosts.ReplaceOneAsync(filter, post, new ReplaceOptions(), cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<SocialPost>.Filter.Eq(p => p.Id, id);
        await _socialPosts.DeleteOneAsync(filter, cancellationToken);
    }
}
