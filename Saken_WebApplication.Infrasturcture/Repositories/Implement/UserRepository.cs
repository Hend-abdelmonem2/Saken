using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDBContext _dbContext;
    public UserRepository(UserManager<User> userManager, ApplicationDBContext dbContext)
    {
        _userManager = userManager;
            _dbContext = dbContext;
    }

    public async Task<IdentityResult> CreateUserAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<User> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<User> FindByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }
    public async Task AddToRoleAsync(User user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }
        public async Task<User?> FindByEmailAndRoleAsync(string email, string role)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Role == role);
        }
        public async Task<User> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User?> FindByPhoneAndRoleAsync(string phone, string role)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phone && u.Role == role);
        }
        public async Task<User> FindByPhoneAsync(string phone)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone);
        }

        public async Task UpdateAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }


        public async Task<List<User>> SearchUsersAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<User>();

            return await _dbContext.Users
                .Where(u => u.UserName.Contains(keyword) || u.Email.Contains(keyword))
                .ToListAsync();
        }

        public async Task<List<User>> FilterUsersAsync(string? name, string? roleName)
        {
            var query = _dbContext.Users.AsQueryable();

            // فلترة بالاسم
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(u => u.UserName.Contains(name));

            // فلترة بالدور
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                query = from user in query
                        join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                        join role in _dbContext.Roles on userRole.RoleId equals role.Id
                        where role.Name == roleName
                        select user;
            }

            return await query.ToListAsync();
        }
    }
}
