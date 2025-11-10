using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Saken_WebApplication.Core.Features.AdditionalInformation.Command.Handlers;
using Saken_WebApplication.Core.Features.Agent.Command.Handlers;
using Saken_WebApplication.Core.Features.Auth.Command.Handlers;
using Saken_WebApplication.Core.Features.Contacts.Command.Handlers;
using Saken_WebApplication.Core.Features.Houses.Command.Handlers;
using Saken_WebApplication.Core.Features.Likes.Command.Handlers;
using Saken_WebApplication.Core.Features.Message.Command.Handlers;
using Saken_WebApplication.Core.Features.Notification.Command.Handlers;
using Saken_WebApplication.Core.Features.Profile.Query.Handlers;
using Saken_WebApplication.Core.Features.Recommendation.Query.Handlers;
using Saken_WebApplication.Core.Features.Reservation.Command.Handlers;
using Saken_WebApplication.Core.Features.Review.Command.Handlers;

namespace Saken_WebApplication.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationMediatR(this IServiceCollection services)
        {

            services.AddMediatR(typeof(Saken_WebApplication.Core.Features.Houses.Command.Handlers.AddHousingHandler).Assembly);
            services.AddMediatR(typeof(AddContactHandler).Assembly);
            services.AddMediatR(typeof(ToggleLikeHandler).Assembly);
            services.AddMediatR(typeof(SendMessageCommandHandler).Assembly);
            services.AddMediatR(typeof(AddReviewHandler).Assembly);
            services.AddMediatR(typeof(AddReservationHandler).Assembly);
            services.AddMediatR(typeof(GetRecommendedHousesHandler).Assembly);
            services.AddMediatR(typeof(SendNotificationHandler).Assembly);
            services.AddMediatR(typeof(AddReviewHandler).Assembly);
            services.AddMediatR(typeof(SavePreferencesHandler).Assembly);
            services.AddMediatR(typeof(CreateOfferHandler).Assembly);
            services.AddMediatR(typeof(RegisterHandler).Assembly);
            services.AddMediatR(typeof(UpdateCommissionStatusHandler).Assembly);
            services.AddMediatR(typeof(GetUserProfileHandler).Assembly);



            return services;
        }
    }
}
