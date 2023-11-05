using DogeFriendsSharedClassLibrary.Interfaces;
using IdentityService.Data;
using IdentityService.Interfaces;
using IdentityService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddHttpClient<ISettingsService, SettingsService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        public static async Task<IServiceCollection> AddCustomIdentity(this IServiceCollection services, IConfiguration config)
        {
            var settingsService = services.BuildServiceProvider().GetRequiredService<ISettingsService>();

            // Инициализация настроек JWT
            var encryptionKey = config.GetSection("SettingsService:SettingsEncryptionKey").Value!;

            var audience = await settingsService.GetSettingValue(config.GetSection("SettingsService:Audience").Value!,
                                                                        encryptionKey);

            var issuer = await settingsService.GetSettingValue(config.GetSection("SettingsService:Issuer").Value!,
                                                                      encryptionKey);

            var secret = await settingsService.GetSettingValue(config.GetSection("SettingsService:Secret").Value!,
                                                                      encryptionKey);

            var connectionString = await settingsService.GetSettingValue(config.GetSection("SettingsService:ConnectionStringSettingKey").Value!,
                                                                         encryptionKey);
            services.AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                {
                    builder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(typeof(DataContext).Assembly.FullName));
                };                
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                {
                    builder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(typeof(DataContext).Assembly.FullName));
                };
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!))
                };
            });

            return services;
        }
    }
}
