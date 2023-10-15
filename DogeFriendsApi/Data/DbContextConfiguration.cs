using DogeFriendsApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DogeFriendsApi.Data
{
    public static class DbContextConfiguration
    {
        private static string? _connectionString = string.Empty;

        // Метод для конфигурации DbContext - лезем в сторонний сервис, где хранятся настройки и оттуда получаем строку подключения к БД
        public static void ConfigureDbContext(IServiceCollection services, IConfiguration config)
        {
            if (_connectionString.IsNullOrEmpty())
            {
                var settingsService = services.BuildServiceProvider().GetRequiredService<ISettingsService>();
                _connectionString = settingsService.GetConnectionStringAsync(config.GetSection("SettingsService").GetValue<string>("ConnectionStringSettingKey")!).GetAwaiter().GetResult();
            }

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(_connectionString);
            });
        }
    }
}
