using GomMessage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<bool> ExistsByEmailAsync(string email);

        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id); 
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task UpdateUserAsync(User user);
    }
}
