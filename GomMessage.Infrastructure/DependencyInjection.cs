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

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();
            services.AddScoped<IHashPasswordService, HashPasswordService>();
            services.AddTransient<IMailService, MailService>();
            return services;
        }
    }
}
