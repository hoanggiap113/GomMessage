using GomMessage.Application.Interfaces;
using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Application.Tenants.Dtos;
using GomMessage.Domain.Constants;
using GomMessage.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Tenants.Queries
{
    public sealed class GetTenantsQueryHandler : IRequestHandler<GetTenantsQuery, ListTenantResponse>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ICacheService _cacheService;
        private readonly ICurrentUserService _currentUserService;

        public GetTenantsQueryHandler(
            ITenantRepository tenantRepository,
            ICacheService cacheService,
            ICurrentUserService currentUserService
            )
        {
            _tenantRepository = tenantRepository;
            _cacheService = cacheService;
            _currentUserService = currentUserService;
        }
        public async Task<ListTenantResponse> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
        {
            //Check user permission
            string userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                throw new UnauthorizedAccessException("Unauthorized access.");
            }

            string cacheKey = CacheKey.UserTenants(userId);
            List<TenantResponse>? cachedTenants = await _cacheService.GetAsync<List<TenantResponse>>(cacheKey);

            //Check cache data for tenants
            if (cachedTenants != null)
            {
                return new ListTenantResponse
                {
                    Data = cachedTenants,
                    Total = cachedTenants.Count,
                    Page = request.Page,
                    Limit = request.Limit
                };
            }

            List<Tenant> tenants = await _tenantRepository.GetListTenantsAsync(
                new TenantFilterDto { UserId = userId },
                cancellationToken
            );
            List<TenantResponse> tenantResponses = tenants.Select(t => new TenantResponse(
                    t.Id,
                    t.Name,
                    t.Slug.Value,
                    t.Settings?.ToString() ?? string.Empty
                )).ToList();
            await _cacheService.SetAsync(cacheKey, tenantResponses, TimeSpan.FromMinutes(30));

            return new ListTenantResponse
            {
                Data = tenantResponses,
                Total = tenantResponses.Count,
                Page = request.Page,
                Limit = request.Limit
            };

        }
    }
}
