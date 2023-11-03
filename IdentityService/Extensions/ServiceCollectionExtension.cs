using DogeFriendsSharedClassLibrary.Interfaces;
using IdentityService.Data;
using IdentityService.Interfaces;
using IdentityService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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

            var audience = await settingsService.GetSettingValue(config.GetSection("SettingsService:Audience").Value!,
                                                                 config.GetSection("SettingsService:SettingsEncryptionKey").Value!);

            var issuer = await settingsService.GetSettingValue(config.GetSection("SettingsService:Issuer").Value!,
                                                               config.GetSection("SettingsService:SettingsEncryptionKey").Value!);

            var secret = await settingsService.GetSettingValue(config.GetSection("SettingsService:Secret").Value!,
                                                               config.GetSection("SettingsService:SettingsEncryptionKey").Value!);

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
                options.TokenValidationParameters = new TokenValidationParameters // Все ключи тоже хранить в SettingsService
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
                    RequireExpirationTime = true
                };
            });

            return services;
        }
    }
}
