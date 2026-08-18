using GomMessage.Application.Interfaces.Repositories;
using GomMessage.Domain.Entities;
using GomMessage.Domain.Entities.Enums;
using GomMessage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Infrastructure.Repository
{
    
    public class InvitationRepository : IInvitationRepository
    {
        private readonly AppDbContext _dbContext;
        public InvitationRepository( AppDbContext context)
        {
            _dbContext = context;
        }
        public void AddInvitation(Invitation invitation)
        {
            _dbContext.Invitations.Add(invitation);
        }

        public Task<Invitation?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Invitation>> GetPendingByTenantIdAsync(Guid tenantId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Invitation?> GetPendingInvitationAsync(Guid tenantId, string email, CancellationToken ct = default)
        {
            return await _dbContext.Invitations
                .Where(i => i.TenantId == tenantId && i.Email == email && i.Status == InvitationStatus.Pending)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> HasPendingInvitationAsync(Guid tenantId, string email, CancellationToken ct = default)
        {
            return await _dbContext.Invitations
                .AnyAsync(i => i.TenantId == tenantId && i.Email == email && i.Status == InvitationStatus.Pending, ct);
        }

        public void Update(Invitation invitation)
        {
            _dbContext.Invitations.Update(invitation);
        }
    }
}
