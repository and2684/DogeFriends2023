﻿using StackExchange.Redis;

namespace SettingsService.Configuration;

public class RedisSetup
{
    private readonly IConfiguration _configuration;

    public RedisSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDatabase InitializeRedisSettings(string databaseName)
    {
        var redisSettings = _configuration.GetSection("RedisSettingsServer");
        string redisServer = redisSettings["Server"]!;
        int redisPort = int.Parse(redisSettings["Port"]!);
        var databaseMappings = redisSettings.GetSection("Databases");

        // Получите номер базы данных по имени
        int databaseNumber = int.Parse(databaseMappings[databaseName]!);

        var redis = ConnectionMultiplexer.Connect($"{redisServer}:{redisPort}");
        return redis.GetDatabase(databaseNumber);
    }
}