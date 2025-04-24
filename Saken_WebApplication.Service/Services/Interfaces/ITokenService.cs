using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateJwtToken(User user);
    }
}
