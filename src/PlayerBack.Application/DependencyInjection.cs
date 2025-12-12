using Microsoft.Extensions.DependencyInjection;
using PlayerBack.Application.Services.PlayerNs;
using PlayerBack.Application.Services.PlayerNs.DataAccess;

namespace PlayerBack.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IPlayerDataAccess, PlayerDataAccess>();

            return services;
        }
    }
}