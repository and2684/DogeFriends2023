using DogeFriendsApi.Data;
using DogeFriendsApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DogeFriendsApi.Configuration
{
    public static class DbContextConfiguration
    {
        private static string? _connectionString = string.Empty;

        // Метод для конфигурации DbContext - лезем в SettingsService, где хранятся настройки и оттуда получаем строку подключения к БД
        public static void ConfigureDbContext(IServiceCollection services, IConfiguration config)
        {
            if (_connectionString.IsNullOrEmpty())
            {
                var settingsService = services.BuildServiceProvider().GetRequiredService<ISettingsService>();
                _connectionString = settingsService.GetConnectionStringAsync(config.GetSection("SettingsService").GetValue<string>("ConnectionStringSettingKey")!).GetAwaiter().GetResult();
            }

            services.AddDbContext<DataContext>(options =>
            {
                //options.UseSqlServer(_connectionString);
                options.UseNpgsql(_connectionString);
            });
        }
    }
}
