using Microsoft.Extensions.DependencyInjection;
using PlayerBack.Application.Services.Player;

namespace PlayerBack.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPlayerService, PlayerService>();
            return services;
        }
    }
}