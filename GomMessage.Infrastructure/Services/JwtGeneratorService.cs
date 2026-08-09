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

    public class JwtGeneratorService : IJwtGeneratorService
    {
        private readonly ILogger<JwtGeneratorService> _logger;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public JwtGeneratorService(ILogger<JwtGeneratorService> logger, IOptions<JwtSettings> jwtSettings)
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

            // 3. Khởi tạo Token Descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                Issuer = _jwtSettings.Value.Issuer,
                Audience = _jwtSettings.Value.Audience,
                SigningCredentials = creds
            };

            // 4. Sinh ra chuỗi JWT
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
        public LoginResponse GenerateToken(string userId, string email, string name)
        {
            var (accessToken, accessTokenExpiresAt) = GenerateAccessToken(userId, email, name);
            var (refreshToken, refreshTokenExpiresAt) = GenerateRefreshToken();
            LoginResponse response = new LoginResponse(
                 AccessToken: accessToken,
                 RefreshToken: refreshToken,
                 AccessTokenExpiresAt: accessTokenExpiresAt,
                 RefreshTokenExpiresAt: refreshTokenExpiresAt
             );
            return response;
        }
    }
}
