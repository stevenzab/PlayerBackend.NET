using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PlayerBack.Domain.Settings;
using PlayerBack.Infrastructure.Common;

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
            return services;
        }
    }
}