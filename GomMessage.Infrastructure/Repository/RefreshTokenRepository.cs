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
        public async Task CreateTokenAsync(string token, Guid userId, DateTime expiration, CancellationToken ct)
        {
            var refreshToken = RefreshToken.Create(userId:userId, tokenHash:token, expiresAt:expiration);
            await _context.RefreshTokens.AddAsync(refreshToken, ct);
            await _context.SaveChangesAsync(ct);
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

        public async Task RevokeRefreshToken(string token, CancellationToken ct)
        {
            var existRt = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == token, ct);
            if (existRt != null)
            {
                _context.RefreshTokens.Remove(existRt);
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}
