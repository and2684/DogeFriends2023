using DogeFriendsApi.Configuration;
using DogeFriendsApi.Data;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Services;
using DogeFriendsSharedClassLibrary.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Text;

namespace DogeFriendsApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDogeFriendsServices(this IServiceCollection services)
        {
            services.AddHttpClient<ISettingsService, SettingsService>();
            services.AddScoped<ISettingsService, SettingsService>();

            services.AddScoped<ICoatsRepository, CoatsRepository>();
            services.AddScoped<ISizesRepository, SizesRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IFriendshipsRepository, FriendshipsRepository>();
            services.AddScoped<IBreedsRepository, BreedsRepository>();
            services.AddScoped<IDogsRepository, DogsRepository>();
            services.AddHttpClient<ISeedService, SeedService>();
            services.AddScoped<ISeedService, SeedService>();
            services.AddAutoMapper(typeof(AutomapperProfiler).Assembly);

            services.AddSingleton<ILoggerManager, LoggerManager>();
            LogManager.Setup().LoadConfigurationFromFile("nlog.config");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "DogeFriendsIssuer",
                    ValidAudience = "DogeFriendsAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DogeFriendsSecret"))
                };
            });

            return services;
        }
    }
}
