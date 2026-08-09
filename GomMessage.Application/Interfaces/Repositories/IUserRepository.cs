using GomMessage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user,CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct); 
        Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct);
        Task UpdateUserAsync(User user, CancellationToken ct);
    }
}
