using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Auth.Dtos
{
    public sealed record TokenResponse(
        string AccessToken, 
        string RefreshToken, 
        DateTime AccessTokenExpiresAt, 
        DateTime RefreshTokenExpiresAt
        );
    //public sealed record RefreshTokenRequest(string refreshToken, string);

}
