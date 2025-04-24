using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByUsernameAsync(string username);
        Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateUserAsync(User user, string password);
        Task AddToRoleAsync(User user, string role);
        Task<User?> FindByEmailAndRoleAsync(string email, string role);
    }
}
