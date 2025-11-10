using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement
{
    public  class AdminRepository: IAdminRepository
    {
        private readonly UserManager<User> _userManager;
       

        public AdminRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await Task.FromResult(_userManager.Users.ToList());
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<List<User>> GetUsersByRoleAsync(string roleName)
        {
            return await _userManager.Users
                .Where(u => u.Role == roleName)
                .ToListAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<int> GetCountUserAsync()
        {
            var users = await GetAllUsersAsync();
            return users.Count();
        }



    }
    }
