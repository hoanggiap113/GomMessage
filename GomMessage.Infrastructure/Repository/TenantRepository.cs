using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Domain.Entities;
using GomMessage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Task GetListTenantsAsync(Tenant[] tenants, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<Tenant?> GetTenantById(string id, CancellationToken ct)
        {
            return await _appDbContext.Tenants.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id),ct);
        }
        public async Task<Tenant?> GetTenantByNameAsync(string name, CancellationToken ct)
        {
            return await _appDbContext.Tenants.FirstOrDefaultAsync(x => x.Name == name,ct);

        }
    }
}
