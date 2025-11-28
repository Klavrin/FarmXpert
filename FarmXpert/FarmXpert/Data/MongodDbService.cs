using MongoDB.Driver;

namespace FarmXpert.Data;

public class MongodDbService
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase? _database;

    public MongodDbService(IConfiguration configuration)
    {
        _configuration = configuration;

        var connectionString = _configuration.GetConnectionString("MongoDb")
                            ?? _configuration.GetConnectionString("DefaultConnection");
        var mongoUrl = MongoUrl.Create(connectionString);
        var mongoClient = new MongoClient(mongoUrl);
        _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
    }

    public IMongoDatabase? Database => _database;
}
