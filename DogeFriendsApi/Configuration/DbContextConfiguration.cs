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
        public static async Task ConfigureDbContextAsync(IServiceCollection services, IConfiguration config)
        {
            if (_connectionString.IsNullOrEmpty())
            {
                var settingsService = services.BuildServiceProvider().GetRequiredService<ISettingsService>();
                _connectionString = await settingsService.GetConnectionStringAsync(
                    config.GetSection("SettingsService:ConnectionStringSettingKey").Value!,
                    config.GetSection("SettingsService:SettingsEncryptionKey").Value!
                );
            }

            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(_connectionString);
            });
        }
    }
}
