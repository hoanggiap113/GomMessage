using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data;
using GomMessage.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GomMessage.Infrastructure.Repository
{
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _appDbContext;
        public TenantRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void CreateTenant(Tenant tenant)
        {
            _appDbContext.Tenants.Add(tenant);

        }
        public async Task<List<Tenant>> GetListTenantsAsync(object? filter, CancellationToken ct)
        {
            return await _appDbContext.Tenants
                .AsNoTracking()
                .ApplyDynamicFilter(filter)
                .ToListAsync(ct);
        }

        public async Task<Tenant?> GetTenantById(string id, CancellationToken ct)
        {
            return await _appDbContext.Tenants.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id),ct);
        }
        public async Task<Tenant?> GetTenantByNameAsync(string name, CancellationToken ct)
        {
            return await _appDbContext.Tenants.FirstOrDefaultAsync(x => x.Name == name,ct);

        }

        public async Task<bool> IsUserExistInTenantAsync(Guid tenantId, string userId, CancellationToken ct)
        {
            return await _appDbContext.UserTenants.AnyAsync(x => x.TenantId == tenantId && x.UserId == Guid.Parse(userId), ct);
        }
    }
}
