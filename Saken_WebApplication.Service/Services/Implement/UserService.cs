using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement
{
    public  class UserService: IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<UpdateUserSettingsDto> GetSettingsAsync(string userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null) return null;

            return new UpdateUserSettingsDto
            {
                IsNotificationsEnabled = user.IsNotificationsEnabled,
                ThemeMode = user.ThemeMode
            };
        }
        public async Task<bool> UpdateSettingsAsync(string userId, UpdateUserSettingsDto dto)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsNotificationsEnabled = dto.IsNotificationsEnabled;
            user.ThemeMode = dto.ThemeMode;

            await _repository.UpdateAsync(user);
            return true;
        }
    }
}
