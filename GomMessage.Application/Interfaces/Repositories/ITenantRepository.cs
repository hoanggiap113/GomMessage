using GomMessage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Interfaces.Repositories
{
    public interface ITenantRepository
    {
        void CreateTenant(Tenant body);
        Task GetListTenantsAsync(Tenant[] tenants, CancellationToken ct);
        Task<Tenant?> GetTenantById(string id,  CancellationToken ct);
        Task<Tenant?> GetTenantByNameAsync(string name, CancellationToken ct);

    }
}
