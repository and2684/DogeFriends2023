using DogeFriendsApi.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;
using DogeFriendsApi.Extensions;
using DogeFriendsApi.Middleware;

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
            //builder.Add; // !!! �������� ������

            builder.Services.AddDogeFriendsServices(); // ��� ������� ���������� ����� � ����� ����������

            await DbContextConfiguration.ConfigureDbContextAsync(builder.Services, builder.Configuration); // ����� ������ ��� ��������� DbContext - � ������� ���� �� ���������� ConnectionString �� SettingsService

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "DogeFriendsApi",
                    Description = "�������� ������ ����������. ������������, ������, ������ � �� ��������."
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

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();

            await app.RunAsync();
        }
    }

}