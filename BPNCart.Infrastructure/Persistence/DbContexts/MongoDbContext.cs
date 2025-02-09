using BPNCart.Application.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BPNCart.Infrastructure.Persistence.DbContexts;
public class MongoDbContext
{
    private readonly IMongoDatabase _mongoDatabase;

    public MongoDbContext(IMongoClient mongoClient, IOptions<MongoDbOptions> options)
    {
        if (options?.Value == null)
            throw new ArgumentNullException(nameof(options), "MongoDB options cannot be null.");

        if (string.IsNullOrEmpty(options.Value.DatabaseName))
            throw new ArgumentException("Database name must be specified in configuration.");

        _mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName)
                    ?? throw new Exception($"Could not connect to MongoDB database: {options.Value.DatabaseName}");
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName = null)
    {
        collectionName ??= typeof(T).Name;
        return _mongoDatabase.GetCollection<T>(collectionName);
    }
}
