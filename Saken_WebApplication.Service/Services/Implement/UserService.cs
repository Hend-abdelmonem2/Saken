using Microsoft.AspNetCore.Http;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;
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
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(IUserRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<List<UserDto>>> SearchUsersAsync(string keyword)
        {
            var users = await _repository.SearchUsersAsync(keyword);

            var Users= users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.UserName,
                Email = u.Email,
                Role = u.Role,
                profilePicture = u.profilePicture,
                IsActive = u.IsActive,

            }).ToList();

            return BaseResponse<List<UserDto>>.SuccessResponse(Users,"Retive success");
        }


        public async Task<BaseResponse<List<UserDto>>> FilterUsersAsync(string? name, string? roleName)
        {
            var isAdmin = _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
            if (!isAdmin)
                throw new UnauthorizedAccessException("🚫 غير مسموح إلا للمشرف (Admin)");

            var users = await _repository.FilterUsersAsync(name, roleName);

            var Users= users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.UserName,
                Email = u.Email,
                Role = u.Role,
                profilePicture = u.profilePicture,
                IsActive = u.IsActive,
            }).ToList();

            return BaseResponse<List<UserDto>>.SuccessResponse(Users, "Retrive success");
        }
    }
}
