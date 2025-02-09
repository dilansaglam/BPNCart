using BPNCart.Application.Options;
using BPNCart.Infrastructure.Persistence.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BPNCart.Infrastructure;
public static class ServiceRegistration
{
    public static void AddInfrastructureService(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<MongoDbOptions>(sp => configuration.GetSection("MongoDb").Bind(sp));

        serviceCollection.AddSingleton<IMongoClient>(sp => new MongoClient(configuration.GetConnectionString("MongoDB")));
        serviceCollection.AddSingleton<MongoDbContext>();
    }
}
