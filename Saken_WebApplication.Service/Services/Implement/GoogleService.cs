using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Service.Response;
using Saken_WebApplication.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement
{
    public class GoogleService: IGoogleService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        public GoogleService(IConfiguration configuration, UserManager<User> userManager, ITokenService tokenService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<BaseResponse<TokenDTO>> GoogleSignInAsync(string TokenId)
        {
            var payload = await VerifyGoogleToken(TokenId);
            if (payload == null)
                return new BaseResponse<TokenDTO>(false, "Failed to validate Google ID token");

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    UserName = payload.Email,
                    Email = payload.Email
                };
                await _userManager.CreateAsync(user);
            }
            var jwtSecurityToken = await _tokenService.CreateJwtToken(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(jwtSecurityToken);

            var res = new TokenDTO
            {
                UserId = user.Id,
                Email = user.Email,

                Name = user.UserName,
                Token = tokenString
            };

            return new BaseResponse<TokenDTO>(true, "تم تسجيل الدخول بنجاح", res);

        }
        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _configuration["Authentication:Google:ClientId"] }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch
            {
                return null;
            }
        }
    }
}
