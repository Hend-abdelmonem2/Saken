using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Auth.Command.Models;
using Saken_WebApplication.Core.Features.Auth.Query.Models;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Service.Services.Interfaces;
using System.Security.Claims;

namespace Saken_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authcontroller : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IGoogleService _googleService;
        private readonly IExecutionContextAccessor _executionContextAccessor;




        public Authcontroller(IMediator mediator, IGoogleService googleService, IExecutionContextAccessor executionContextAccessor)
        {
            _mediator = mediator;
            _googleService = googleService;
            _executionContextAccessor = executionContextAccessor;

        }




        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModelDTO model)
        {
            var result = await _mediator.Send(new RegisterCommand(model));
            if (!result.Success)
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


            var result = await _mediator.Send(new LoginCommand(loginRegister));

            if (!result.Success)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.Data.RefreshToken))
                SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpiration);

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
                var result = await _mediator.Send(new ForgetPasswordCommand(email));
                return Ok(result);
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

        [HttpPost("ResetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _mediator.Send(new ResetPasswordCommand(model));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("refreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));

            if (!result.Success)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("revokeToken")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _mediator.Send(new RevokeTokenCommand(model.Token));

            if (!result.Success)
                return BadRequest("Token is invalid!");

            return Ok(result);
        }


        [HttpPost("Send2FACode/{email}")]
        [Authorize]
        public async Task<IActionResult> SendTwoFactorCode(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _mediator.Send(new Send2FACodeCommand(email));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("ReSend2FACode/{email}")]
        [Authorize]
        public async Task<IActionResult> ReSendTwoFactorCode(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _mediator.Send(new Resend2FACodeCommand(email));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Verify2FACode")]
        [Authorize]
        public async Task<IActionResult> VerifyTwoFactorCode([FromBody] Verify2FACodeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _mediator.Send(new Verify2FACodeCommand(model));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(new UpdateProfileCommand(userId, model));

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }

        [HttpPut("UpdateRole")]
        [Authorize]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _mediator.Send(new UpdateRoleCommand(model));
                return Ok(result);
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
            var userId = _executionContextAccessor.UserId;
            try
            {
                var users = await _mediator.Send(new GetUsersQuery(userId));
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
            var userId = _executionContextAccessor.UserId;
            try
            {
                var users = await _mediator.Send(new GetUsersByRoleQuery(userId, roleName));
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UserById")]
        [Authorize]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetUserById(string Id)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByIdQuery(Id));
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("signout")]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            var accessToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _executionContextAccessor.UserId;

            var response = await _mediator.Send(new LogoutCommand(accessToken, userId));

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
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

    }
}
