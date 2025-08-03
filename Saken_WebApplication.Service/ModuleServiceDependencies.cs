using Microsoft.Extensions.DependencyInjection;
using Saken_WebApplication.Service.Services.Implement;
using Saken_WebApplication.Service.Services.Implement.housing;
using Saken_WebApplication.Service.Services.Implement.Like;
using Saken_WebApplication.Service.Services.Implement.message;
using Saken_WebApplication.Service.Services.Implement.Recommand;
using Saken_WebApplication.Service.Services.Implement.Reservation;
using Saken_WebApplication.Service.Services.Implement.UserPreference;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using Saken_WebApplication.Service.Services.Interfaces.Like;
using Saken_WebApplication.Service.Services.Interfaces.message;
using Saken_WebApplication.Service.Services.Interfaces.recommend;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using Saken_WebApplication.Service.Services.Interfaces.UserPreferences;

namespace Saken_WebApplication.Service
{
    public static  class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceeDependencies(this IServiceCollection services)
        {
            services.AddTransient<IHousingService, HousingService>();
            services.AddTransient<ILikeService,LikeService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IRecommendationService,RecommendationService>();
            services.AddTransient<IReservationService,ReservationService>();
            services.AddTransient<IUserPreferencesService, UserPreferencesService>();
            services.AddTransient<IAppSettingsService, AppSettingsService >();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IDummyDataService, DummyDataService>();
            services.AddTransient<IDummyUserService, DummyUserService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IGoogleService,GoogleService>();
            services.AddTransient<IHousingFilterService, HousingFilterService>();
            services.AddTransient<ITokenService, TokenService > ();
            services.AddTransient<ITokenBlacklistService, TokenBlacklistService>();
            services.AddTransient<IUserService, UserService>();
            return services;
        }

    }
}
