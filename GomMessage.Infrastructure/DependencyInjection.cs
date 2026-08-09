using GomMessage.Application.Interfaces;
using GomMessage.Infrastructure.Repository;
using GomMessage.Infrastructure.Services;
using GomMessage.Infrastructure.Services.Cache;
using GomMessage.Infrastructure.Services.Email;
using GomMessage.Infrastructure.Settings;
using GomMessage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GomMessage.Application.Interfaces.Repositories;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GomMessage.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
     
            //Database Configuration
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            //Redis Server Configuration
            var redisSection = configuration.GetSection("RedisCacheSettings");
            var redisSettings = redisSection.Get<RedisSettings>() ?? new RedisSettings();
            services.Configure<RedisSettings>(redisSection);
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings.ConnectionString;
                options.InstanceName = redisSettings.InstanceName;
            });


            //MailService Configuration
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            //Authentication
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (string.IsNullOrEmpty(context.Token))
                            {
                                context.Request.Cookies.TryGetValue("access_token", out var accessToken);
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();
            services.AddScoped<IHashPasswordService, HashPasswordService>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            return services;
        }
    }
}
