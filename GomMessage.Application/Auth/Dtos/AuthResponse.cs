using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Auth.Dtos
{
    public sealed record AuthResponse(string accessToken,string refreshToken);
    
}
