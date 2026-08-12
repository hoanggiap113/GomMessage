using GomMessage.Application;
using GomMessage.Application.Auth.Commands;
using GomMessage.Application.Auth.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GomMessage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginCommand request, CancellationToken cancellationToken)
        //{
        //    // Implement login logic here
        //    var result = await _sender.Send(request, cancellationToken);
        //    return Ok();
        //}
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest dto,CancellationToken ct)
        {
            var command = new RegisterCommand(dto.Email, dto.Password, dto.Name, dto.Telephone);
            var result = await _mediator.Send(command);
            return Ok(new
            {
                success = true,
                message = "User registered successfully please verify your email",
                data = result
            });
        }
        [HttpPost("verify-otp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest dto, CancellationToken ct)
        {
            var command = new VerifyOtpCommand(dto.Email, dto.Otp);
            var result = await _mediator.Send(command);
            return Ok(new
            {
                success = true,
                message = "User registered successfully please verify your email",
                data = result
            });
        }
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken(CancellationToken ct)
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Refresh token is missing"
                });
            }
            var command = new RefreshTokenCommand(refreshToken);
            var result = await _mediator.Send(command, ct);
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = result.AccessTokenExpiresAt
            };
            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = result.RefreshTokenExpiresAt
            };
            Response.Cookies.Append("access_token", result.AccessToken, accessTokenCookieOptions);
            Response.Cookies.Append("refresh_token", result.RefreshToken, refreshTokenCookieOptions);
            return Ok(new
            {
                success = true,
                message = "Access token refreshed successfully",
            });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginCommand dto, CancellationToken ct)
        {
            var result = await _mediator.Send(dto);
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,            
                Secure = true,              
                SameSite = SameSiteMode.Lax,
                Expires = result.AccessTokenExpiresAt
            };
            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = result.RefreshTokenExpiresAt
            };
            Response.Cookies.Append("access_token", result.AccessToken, accessTokenCookieOptions);
            Response.Cookies.Append("refresh_token", result.RefreshToken, refreshTokenCookieOptions);
            return Ok(new
            {
                success = true,
                message = "User logged in successfully",
            });
        }
    }
}
