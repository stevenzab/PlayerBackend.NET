using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PlayerBack.Domain.Settings;
using PlayerBack.Infrastructure.Common;
using PlayerBack.Infrastructure.Seeding;

namespace PlayerBack.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var mongodbSettings = configuration
               .GetSection("MongoDB")
               .Get<MongoSettings>();

            if (mongodbSettings == null)
                throw new InvalidOperationException("MongoDB settings are not configured properly.");

            var mongoClient = new MongoClient(mongodbSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongodbSettings.DatabaseName);

            services.AddSingleton(mongoDatabase);
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IDbSeeder, PlayerDbSeeder>();
            return services;
        }
    }

    public static class DbSeederExtensions
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();

            if (!await seeder.HasDataAsync())
                await seeder.SeedAsync();
        }
    }
}