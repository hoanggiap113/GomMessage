using GomMessage.Application.Interfaces;
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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }   
        public async Task<User> CreateAsync(User user, CancellationToken ct)
        {
            var entityEntry = await _context.Users.AddAsync(user, ct);
            await _context.SaveChangesAsync(ct);
            return entityEntry.Entity;
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _context.Users.AnyAsync(u => u.Email == email, ct);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
