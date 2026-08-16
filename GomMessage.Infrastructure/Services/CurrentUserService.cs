using GomMessage.Application.Interfaces;
using GomMessage.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;

        }

        public string? UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                return user?.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? user?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                       ?? user?.FindFirstValue("sub");
            }
        }
    }
}
