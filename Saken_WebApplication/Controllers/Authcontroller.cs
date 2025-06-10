using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Service.Services.Implement;
using Saken_WebApplication.Service.Services.Interfaces;
using System.Security.Claims;

namespace Saken_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authcontroller : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGoogleService _googleService;

        public Authcontroller(IAuthService authService, IGoogleService googleService)
        {
            _authService = authService;
            _googleService = googleService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModelDTO model)
        {
            var result = await _authService.RegisterAsync(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(new { message = result.Message });
            }


            return Ok(result);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] RequestLoginDto loginRegister)
        {
            if (!ModelState.IsValid)
            
                return BadRequest(ModelState);
            
            
                var result = await _authService.LoginAsync(loginRegister);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);

                if (!string.IsNullOrEmpty(result.RefreshToken))
                    SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

                return Ok(result);
            
        }
        [HttpPost("ForgetPassword/{email}")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.ForgetPasswordAsync(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.ResetPasswordAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        [HttpPost("Send2FACode/{email}")]
        public async Task<IActionResult> SendTwoFactorCode(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.Send2FACodeAsync(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ReSend2FACode/{email}")]
        public async Task<IActionResult> ReSendTwoFactorCode(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.Resend2FACodeAsync(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Verify2FACode")]
        public async Task<IActionResult> VerifyTwoFactorCode([FromBody] Verify2FACodeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                string result = await _authService.Verify2FACodeAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // أو حسب الطريقة اللي بتجيبي بيها ID المستخدم

            var result = await _authService.UpdateProfileAsync(userId, model);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _authService.UpdateRoleAsync(model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("AllUsers")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var users = await _authService.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("AllUsersByRole/{roleName}")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var users = await _authService.GetUsersByRoleAsync(roleName);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UserById")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetUserById()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier); // أو حسب الطريقة اللي بتجيبي بيها ID المستخدم
            try
            {
                var user = await _authService.GetUserByIdAsync(Id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(string idToken)
        {
            var result = await _googleService.GoogleSignInAsync(idToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
