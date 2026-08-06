using GomMessage.Application.Auth.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Interfaces
{
    public interface IJwtGeneratorService
    {
        LoginResponse GenerateToken(string userId, string email, string name);
    }
}
