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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }   
        public void AddUser(User user)
        {
            var entityEntry = _context.Users.AddAsync(user);
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

        public void UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

    }
}
