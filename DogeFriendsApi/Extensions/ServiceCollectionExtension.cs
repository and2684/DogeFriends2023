﻿using DogeFriendsApi.Configuration;
using DogeFriendsApi.Data;
using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Services;

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

            return services;
        }
    }
}
