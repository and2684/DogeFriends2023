using DogeFriendsSharedClassLibrary.Interfaces;
using IdentityService.Configuration;
using IdentityService.Extensions;

namespace IdentityService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddCustomServices();
            await DbContextConfiguration.ConfigureDbContextAsync(builder.Services, builder.Configuration); // Вызов метода для настройки DbContext - с помощью него мы вычитываем ConnectionString из SettingsService

            await builder.Services.AddCustomIdentity(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}