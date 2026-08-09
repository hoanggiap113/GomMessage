using GomMessage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task CreateTokenAsync(string token, Guid userId, DateTime expiration, CancellationToken ct);

        Task<RefreshToken?> GetByUserId(Guid userId, CancellationToken ct);
        Task<RefreshToken?> GetRefreshTokenByToken(string token, CancellationToken ct);    
        Task RevokeRefreshToken(string token, CancellationToken ct);
    }
}
