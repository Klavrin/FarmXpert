using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MongoDB.Driver;

namespace FarmXpert.Infrastructure.Repositories;

public class TodoRepository: ITodoRepository
{
    private readonly IMongoCollection<Todo> _todos;
    
    public TodoRepository(IMongoDatabase database)
    {
        _todos = database.GetCollection<Todo>("todos");
    }
    
    public async Task CreateAsync(Todo todo, CancellationToken cancellationToken = default)
    {
        await _todos.InsertOneAsync(todo, null, cancellationToken);
    }
    
    public async Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Todo>.Filter.Eq(t => t.Id, id);
        return await _todos.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _todos.Find(_ => true).ToListAsync(cancellationToken);
    }
    
    public async Task UpdateAsync(Todo todo, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Todo>.Filter.Eq(t => t.Id, todo.Id);
        await _todos.ReplaceOneAsync(filter, todo, new ReplaceOptions(), cancellationToken);
    }
    
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Todo>.Filter.Eq(t => t.Id, id);
        await _todos.DeleteOneAsync(filter, cancellationToken);
    }
}