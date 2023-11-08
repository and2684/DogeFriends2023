using IdentityService.Configuration;
using IdentityService.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;

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

            // Добавляем сервисы
            builder.Services.AddCustomServices();
            // Вызов метода для настройки DbContext - с помощью него мы вычитываем ConnectionString из SettingsService
            await DbContextConfiguration.ConfigureDbContextAsync(builder.Services, builder.Configuration);
            // Настройки Identity
            await builder.Services.AddCustomIdentity(builder.Configuration);

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "IdentityService",
                    Description = "Сервис авторизации."
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

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

            app.UseIdentityServer();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}