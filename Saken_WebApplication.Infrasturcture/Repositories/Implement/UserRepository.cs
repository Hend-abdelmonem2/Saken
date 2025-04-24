using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models;
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
    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
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

    }
}
