using Microsoft.AspNetCore.Http;
using Saken_WebApplication.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement
{
    public class ExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }



        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        // ✅ UserId: بياخد من uid لو موجود، وإلا بياخد من NameIdentifier
        public string? UserId =>
            User?.FindFirst("uid")?.Value ??
            User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // ✅ UserName: من Identity.Name أو من الـ claims
        public string? UserName =>
            User?.Identity?.Name ??
            User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // ✅ Email: من claim الـ emailaddress
        public string? Email =>
            User?.FindFirst(ClaimTypes.Email)?.Value;

        // ✅ Role: من claim الـ role
        public string? Role =>
            User?.FindFirst(ClaimTypes.Role)?.Value;

    }
}
