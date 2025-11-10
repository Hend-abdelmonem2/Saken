using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Repositories.Implement;
using Saken_WebApplication.Infrasturcture.Repositories.Implement.contact;
using Saken_WebApplication.Infrasturcture.Repositories.Implement.Guide;
using Saken_WebApplication.Infrasturcture.Repositories.Implement.Notifications;
using Saken_WebApplication.Infrasturcture.Repositories.Implement.Preferences;
using Saken_WebApplication.Infrasturcture.Repositories.Implement.Reservation;
using Saken_WebApplication.Infrasturcture.Repositories.Implement.Review;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Contact;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Guide;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Notifications;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Preferences;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Reservation;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Review;

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
            services.AddTransient<INotificationRepository,NotificationRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<IHousingOfferRepository, HousingOfferRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IReviewRepository, ReviewRepository>();
            services.AddTransient<ICommissionTrackingRepository,CommissionTrackingRepository>();
            
            return services;

        }

    }
}
