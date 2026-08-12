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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
        }

        public async Task<RefreshToken?> GetByUserId(Guid userId, CancellationToken ct)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        }

        public async Task<RefreshToken?> GetRefreshTokenByToken(string token, CancellationToken ct)
        {
            var rt = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == token, ct);
            return rt;
        }

        public void RevokeRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
        }
    }
}
