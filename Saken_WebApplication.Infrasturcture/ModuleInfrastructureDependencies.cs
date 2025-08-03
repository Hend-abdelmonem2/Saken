using Microsoft.Extensions.DependencyInjection;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Repositories.Implement;
using Saken_WebApplication.Infrasturcture.Repositories.Implement.Preferences;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Preferences;

namespace Saken_WebApplication.Infrasturcture
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
           services.AddTransient<Saken_WebApplication.Infrasturcture.Repositories.Interfaces.IHouses, Saken_WebApplication.Infrasturcture.Repositories.Implement.Housing>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserPreferencesRepository, UserPreferencesRepository>();
            services.AddTransient<IHousingRepository, HousingRepository>();
            services.AddTransient<ILikeRepository,LikeRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();

            return services;

        }

    }
}
