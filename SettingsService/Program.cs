using FluentValidation;
using Microsoft.OpenApi.Models;
using SettingsService.Services;
using SettingsService.Services.Interfaces;
using System.Reflection;
using SettingsService.Services.Repository;
using SettingsService.Data.Models;
using SettingsService.Configuration;

namespace SettingsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<RedisSetup>();
            builder.Services.AddScoped<IValidator<Setting>, SettingsValidatorService >();
            builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
            builder.Services.AddDataProtection();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SettingsService",
                    Description = "Сервис для хранения настроек микросервисов приложения."
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

            app.MapControllers();

            app.Run();
        }
    }
}