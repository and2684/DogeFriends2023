using DogeFriendsApi.Configuration;
using DogeFriendsApi.Extensions;
using DogeFriendsApi.Middleware;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DogeFriendsApi
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

            builder.Services.AddDogeFriendsServices(); // Все сервисы приложения вынес в метод расширения

            await DbContextConfiguration.ConfigureDbContextAsync(builder.Services, builder.Configuration); // Вызов метода для настройки DbContext - с помощью него мы вычитываем ConnectionString из SettingsService

            #region Настройки авторизации в Swagger
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JSON Web Token based security"                
            };

            var securityReq = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                        },
                    Array.Empty<string>()
                }
            };
            #endregion

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "DogeFriendsApi",
                    Description = "Основной сервис приложения. Пользователи, собаки, породы и их свойства."
                });

                options.AddSecurityDefinition("Bearer", securityScheme);
                options.AddSecurityRequirement(securityReq);

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

            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();

            await app.RunAsync();
        }
    }

}