using DogeFriendsApi.Configuration;
using DogeFriendsApi.Data;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Services;
using DogeFriendsSharedClassLibrary.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NLog;

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
                options.Authority = "https://localhost:5101"; // URL нашего Identity Server (ХРАНИТЬ В SETTINGS SERVICE)
                options.Audience = "DogeFriendsAudience"; // Значение аудитории должно соответствовать настройкам на стороне сервера (ХРАНИТЬ В SETTINGS SERVICE)
            });

            return services;
        }
    }
}
