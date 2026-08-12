using GomMessage.Application.Tenants.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GomMessage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TenantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return Ok(new
            {
                success = true,
                message = "Tenant created successfully",
                data = result
            });
        }

    }
}
