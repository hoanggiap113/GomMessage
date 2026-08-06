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
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginCommand dto, CancellationToken ct)
        {
            var result = await _mediator.Send(dto);
            return Ok(new
            {
                success = true,
                message = "User logged in successfully",
                data = result
            });
        }
    }
}
