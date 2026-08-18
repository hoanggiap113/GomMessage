using GomMessage.Application.Auth.Dtos;
using GomMessage.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GomMessage.Infrastructure.Services
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationInHours { get; set; } = 24;
        public int RefreshTokenExpirationInDays { get; set; } = 7;
    }

    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public TokenService(ILogger<TokenService> logger, IOptions<JwtSettings> jwtSettings)
        {
            _logger = logger;
            _jwtSettings = jwtSettings;
        }
        private (string token, DateTime expiresAt) GenerateAccessToken(string userId, string email, string name)
        {
            var expiresAt = DateTime.UtcNow.AddHours(_jwtSettings.Value.ExpirationInHours);

            _logger.LogInformation("Generating JWT. Issuer: {Issuer}, Audience: {Audience}, ExpiresAt: {ExpiresAt}",
                _jwtSettings.Value.Issuer, _jwtSettings.Value.Audience, expiresAt);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Name, name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                Issuer = _jwtSettings.Value.Issuer,
                Audience = _jwtSettings.Value.Audience,
                SigningCredentials = creds
            };

            // Jwt generation
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return (tokenString, expiresAt);

        }
        private (string token, DateTime expiresAt) GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var token = Convert.ToBase64String(randomNumber);
            var expiresAt = DateTime.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpirationInDays);

            return (token, expiresAt);
        }
        public TokenResponse GenerateToken(string userId, string email, string name)
        {
            var (accessToken, accessTokenExpiresAt) = GenerateAccessToken(userId, email, name);
            var (refreshToken, refreshTokenExpiresAt) = GenerateRefreshToken();
            TokenResponse response = new TokenResponse(
                 AccessToken: accessToken,
                 RefreshToken: refreshToken,
                 AccessTokenExpiresAt: accessTokenExpiresAt,
                 RefreshTokenExpiresAt: refreshTokenExpiresAt
             );
            return response;
        }

        public (string RawToken, string HashedToken) GenerateInvitationToken()
        {
            byte[] randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Chuyển thành chuỗi URL-Safe Base64 (dùng gửi qua email)
            string rawToken = Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');

            // Hash lại bằng SHA-256 
            string hashedToken = HashInvitationToken(rawToken);
            return (rawToken, hashedToken);
        }

        public string HashInvitationToken(string rawToken)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(rawToken);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hash); 
            }
        }
    }
}
