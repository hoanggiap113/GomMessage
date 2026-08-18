using GomMessage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Interfaces.Repositories
{
    public interface IInvitationRepository
    {  
            void AddInvitation(Invitation invitation);
            Task<Invitation?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default);
            Task<Invitation?> GetPendingInvitationAsync(Guid tenantId, string email, CancellationToken ct = default);
            Task<bool> HasPendingInvitationAsync(Guid tenantId, string email, CancellationToken ct = default);
            Task<List<Invitation>> GetPendingByTenantIdAsync(Guid tenantId, CancellationToken ct = default);
            void Update(Invitation invitation);
        
    }
}
