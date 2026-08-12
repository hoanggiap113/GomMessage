using GomMessage.Application.Auth.Dtos;
using GomMessage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Interfaces
{
    public interface IJwtService
    {
        TokenResponse GenerateToken(string userId, string email, string name);
    }
}
