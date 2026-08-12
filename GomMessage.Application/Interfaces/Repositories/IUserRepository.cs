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
        void AddUser(User user);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct); 
        Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct);
        void UpdateUserAsync(User user);
    }
}
