using Microsoft.AspNetCore.Identity;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement
{
   public class DummyUserService: IDummyUserService
    {
        private readonly UserManager<User> _userManager;

        public DummyUserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> InsertDummyUsersAsync()
        {
            var dummyUsers = new List<RegisterModelDTO>

    {
                  // ==== 8 Owners ====
            new RegisterModelDTO { FullName = "Muhamme_dAhmed", Email = "muhamed1@domain.com", PhoneNumber = "01000000001", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "123 Main Street", Role = "Owner", PhotoUrl="https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Ayman_Omar", Email = "ayman2@domain.com", PhoneNumber = "01000000002", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Maadi Street", Role = "Owner", PhotoUrl="https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Hassan_Ali", Email = "hassan3@domain.com", PhoneNumber = "01000000003", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Zamalek", Role = "Owner", PhotoUrl="https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Omar_Adel", Email = "omar4@domain.com", PhoneNumber = "01000000004", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Nasr City", Role = "Owner", PhotoUrl="https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Sami_Khaled", Email = "sami5@domain.com", PhoneNumber = "01000000005", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Dokki", Role = "Owner", PhotoUrl="https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Ahmed_Hani", Email = "ahmed6@domain.com", PhoneNumber = "01000000006", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "October", Role = "Owner", PhotoUrl="https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Tarek_Nabil", Email = "tarek7@domain.com", PhoneNumber = "01000000007", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Heliopolis", Role = "Owner", PhotoUrl="https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Fady_Youssef", Email = "fady8@domain.com", PhoneNumber = "01000000008", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Rehab", Role = "Owner", PhotoUrl="https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },

    // ==== 8 Tenants ====
            new RegisterModelDTO { FullName = "Omar_Yasser", Email = "tenant1@domain.com", PhoneNumber = "01111111111", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "456 Elm Street", Role = "Tenant", PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Khaled_Ahmed", Email = "tenant2@domain.com", PhoneNumber = "01111111112", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Mohandessin", Role = "Tenant", PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Mostafa_Ali", Email = "tenant3@domain.com", PhoneNumber = "01111111113", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Nasr City", Role = "Tenant", PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Youssef_Amr", Email = "tenant4@domain.com", PhoneNumber = "01111111114", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "October", Role = "Tenant", PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Ibrahim_Adel", Email = "tenant5@domain.com", PhoneNumber = "01111111115", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Helwan", Role = "Tenant", PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Mahmoud_Nabil", Email = "tenant6@domain.com", PhoneNumber = "01111111116", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Rehab", Role = "Tenant", PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Tarek_Samir", Email = "tenant7@domain.com", PhoneNumber = "01111111117", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Zayed", Role = "Tenant", PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" },
            new RegisterModelDTO { FullName = "Hassan_Ehab", Email = "tenant8@domain.com", PhoneNumber = "01111111118", Password = "Pass123!", ConfirmPassword = "Pass123!", address = "Fifth Settlement", Role = "Tenant", PhotoUrl = "https://res.cloudinary.com/dchyk5uyj/image/upload/v1748985910/hqrd66f7jsqutvuseahn.webp" }
};

            foreach (var dto in dummyUsers)
            {
                if (await _userManager.FindByEmailAsync(dto.Email) != null)
                    continue;

                var user = new User
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    UserName = dto.FullName,
                    PhoneNumber = dto.PhoneNumber,
                    address = dto.address,
                    EmailConfirmed = true,
                    Role = dto.Role,
                    profilePicture = dto.PhotoUrl
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, dto.Role);
                }
                else
                {
                    return "Error inserting users: " + string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }

            return "Dummy users inserted successfully.";
        }
        public async Task<string> DeleteDummyUsersAsync()
        {
            var dummyEmails = new List<string>
    {
        "muhamed1@domain.com",
        "ayman2@domain.com",
        "hassan3@domain.com",
        "omar4@domain.com",
        "sami5@domain.com",
        "ahmed6@domain.com",
        "tarek7@domain.com",
        "fady8@domain.com",
        "tenant1@domain.com",
        "tenant2@domain.com",
        "tenant3@domain.com",
        "tenant4@domain.com",
        "tenant5@domain.com",
        "tenant6@domain.com",
        "tenant7@domain.com",
        "tenant8@domain.com"



    };

            var logs = new List<string>();

            foreach (var email in dummyEmails)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                        logs.Add($"✅ Deleted: {email}");
                    else
                        logs.Add($"❌ Failed to delete {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                else
                {
                    logs.Add($"⚠️ Not found: {email}");
                }
            }

            return string.Join("\n", logs);
        }



    }
}

