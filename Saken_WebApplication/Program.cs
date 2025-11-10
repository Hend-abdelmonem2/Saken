using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Saken_WebApplication.Core;
using Saken_WebApplication.Core.Extensions;
using Saken_WebApplication.Core.Features.Houses.Base;
using Saken_WebApplication.Core.Features.Houses.Command.Handlers;
using Saken_WebApplication.Core.Features.Houses.Query.Handlers;
using Saken_WebApplication.Core.Features.Reservation.Command.Handlers;
using Saken_WebApplication.Core.Features.Reservation.Query.Handlers;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories;
using Saken_WebApplication.Infrasturcture.Repositories.Implement;
using Saken_WebApplication.Infrasturcture.Repositories.Implement.Preferences;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Preferences;
using Saken_WebApplication.Service;
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
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new SqlServerStorageOptions
        {
            SchemaName = "Hangfire", 
            QueuePollInterval = TimeSpan.FromSeconds(15),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true
        }
    )
);


builder.Services.AddHangfireServer();

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
#region Dependency Injection
builder.Services.AddInfrastructureDependencies()
    .AddServiceeDependencies()
    .AddCoreDependencies();
#endregion
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationMediatR();
builder.Services.AddHttpClient<IHousingService, HousingService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();






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
app.UseSwagger();
if (app.Environment.IsDevelopment())
    app.UseSwaggerUI();
else
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHangfireDashboard("/dashboard");

app.MapControllers();

app.Run();
