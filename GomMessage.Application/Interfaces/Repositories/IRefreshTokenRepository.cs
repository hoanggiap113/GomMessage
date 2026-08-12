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
        void CreateToken(RefreshToken refreshToken);
        Task<RefreshToken?> GetByUserId(Guid userId, CancellationToken ct);
        Task<RefreshToken?> GetRefreshTokenByToken(string token, CancellationToken ct);    
        void RevokeRefreshToken(RefreshToken refreshToken);
    }
}
