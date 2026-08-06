using GomMessage.Application.Auth.Dtos;
using GomMessage.Application.Interfaces;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpirationInHours);
            _logger.LogInformation("jwt issuer: {issuer}, audience: {audience}, " +
                "expiration: {expiration}", _jwtSettings.Value.Issuer, _jwtSettings.Value.Audience, expiresAt);
            var expUnix = new DateTimeOffset(expiresAt).ToUnixTimeSeconds();
            var token = JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(_jwtSettings.Value.Secret)
            .AddClaim("exp", expUnix)
            .AddClaim("iss", _jwtSettings.Value.Issuer)
            .AddClaim("aud", _jwtSettings.Value.Audience)
            .AddClaim("sub", userId)
            .AddClaim("email", email)
            .AddClaim("name", name)
            .Encode();

            return (token, expiresAt);
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
                 accessToken: accessToken,
                 refreshToken: refreshToken,
                 AccessTokenExpiresAt: accessTokenExpiresAt,
                 RefreshTokenExpiresAt: refreshTokenExpiresAt
             );
            return response;
        }
    }
}
