using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Auth.Dtos
{
    public sealed record UserCache(
        string Name,
        string Email,
        string PasswordHash,
        string Otp,
        string Telephone,
        int FailedAttempts = 0
        );
}
