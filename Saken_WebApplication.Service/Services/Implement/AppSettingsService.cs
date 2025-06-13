using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly ApplicationDBContext _context;

        public AppSettingsService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<AppSettingsDto> GetSettingsAsync()
        {
            var settings = await _context.AppSettings.FirstOrDefaultAsync();
            if (settings == null) return null!;

            return new AppSettingsDto
            {
                AppName = settings.AppName,
                LogoUrl = settings.LogoUrl,
                SupportEmail = settings.SupportEmail,
                EnableNotifications = settings.EnableNotifications,

            };
        }

        public async Task UpdateSettingsAsync(AppSettingsDto dto)
        {
            var settings = await _context.AppSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new AppSettings();
                _context.AppSettings.Add(settings);
            }

            settings.AppName = dto.AppName;
            settings.LogoUrl = dto.LogoUrl;
            settings.SupportEmail = dto.SupportEmail;
            settings.EnableNotifications = dto.EnableNotifications;

            await _context.SaveChangesAsync();
        }
    }
}
