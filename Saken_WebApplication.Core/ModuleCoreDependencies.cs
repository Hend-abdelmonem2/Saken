using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Saken_WebApplication.Core.Extensions;
using System.Reflection;

namespace Saken_WebApplication.Core
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddApplicationMediatR();
            return services;
        }
    }
}
