using GomMessage.Application.Tenants.Commands;
using GomMessage.Application.Tenants.Dtos;
using GomMessage.Application.Tenants.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;


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

        [Authorize]
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTenants(
            CancellationToken ct, 
            [FromQuery] int page = 1, 
            [FromQuery] int limit = 10)
        {
            var result = await _mediator.Send(new GetTenantsQuery(page, limit), ct);

            return Ok(new
            {
                success = true,
                message = "Tenants retrieved successfully",
                data = result
            });

        }

        [Authorize]
        [HttpPost("{tenantId}/invitation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InviteMember(
            [FromRoute] Guid tenantId,
            [FromBody] InviteMemberRequest request,
            CancellationToken ct)
        {
            var command = new InviteMemberCommand(tenantId, request.Email, request.Role);
            var result = await _mediator.Send(command, ct);

            return Ok(new
            {
                success = true,
                message = "Member invited successfully",
                data = result
            });
        }
    }
}
