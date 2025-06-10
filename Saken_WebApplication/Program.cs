using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Implement;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Implement;
using Saken_WebApplication.Service.Services.Interfaces;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Configuration;
using Saken_WebApplication.Service.Services.Implement.Like;
using Saken_WebApplication.Service.Services.Interfaces.Like;
using Saken_WebApplication.Service.Services.Implement.housing;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using Saken_WebApplication.Service.Services.Interfaces.message;
using Saken_WebApplication.Service.Services.Implement.message;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Preferences;
using Saken_WebApplication.Infrasturcture.Repositories.Implement.Preferences;
using Saken_WebApplication.Service.Services.Interfaces.UserPreferences;
using Saken_WebApplication.Service.Services.Implement.UserPreference;
using Saken_WebApplication.Service.Services.Interfaces.recommend;
using Saken_WebApplication.Service.Services.Implement.Recommand;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using Saken_WebApplication.Service.Services.Implement.Reservation;
using AutoMapper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDataProtection();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddIdentityCore<User>()
           .AddRoles<IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDBContext>()
           .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDBContext>();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddSingleton<ICloudinaryService, CloudinaryService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IDummyDataService, Saken_WebApplication.Service.Services.Implement.DummyDataService>();
builder.Services.AddScoped<IDummyUserService, DummyUserService>();



builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHouses, Saken_WebApplication.Infrasturcture.Repositories.Implement.Housing>();
builder.Services.AddScoped<IHousingFilterService, HousingFilterService>();
builder.Services.AddScoped<IHousingService, HousingService>();
builder.Services.AddScoped<IHousingRepository, HousingRepository>();
builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();
builder.Services.AddScoped<IUserPreferencesRepository, UserPreferencesRepository>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IGoogleService, GoogleService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();

builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var tokenBlacklistService = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();
                            var token = context.Token;

                            if (token != null && tokenBlacklistService.IsTokenBlacklisted(token))
                            {
                                context.Fail("This token is blacklisted.");
                            }

                            return Task.CompletedTask;
                        }
                    };

                }).AddGoogle("Google", options =>
                {
                    options.ClientId = "820232595279-8a81v7jjevbvu7emrs33kkf8eeh1bm42.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                    options.CallbackPath = "/signin-google";
                })

  .AddCookie();
  /*.AddGoogle(options =>
  {
      IConfigurationSection googleAuthSection = builder.Configuration.GetSection("Authentication:Google");

      options.ClientId = googleAuthSection["ClientId"];
      options.ClientSecret = googleAuthSection["ClientSecret"];
  });*/

builder.Services.AddSwaggerGen(swagger => {

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }

                    }
               });

});

var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
