using System.Net;
using System.Text.Json;
using DogeFriendsApi.Exceptions;
using DogeFriendsApi.Interfaces;

namespace DogeFriendsApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILoggerManager _logger;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env, ILoggerManager logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message.ToString());

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode,
                        ex.Message.ToString(),
                        ((ex.StackTrace == null) ? string.Empty : ex.StackTrace.ToString())) // Если приложение в разработке - покажем стектрейс
                    : new ApiException(context.Response.StatusCode, "Internal server error", ex.Message.ToString()); // Если в проде - просто ошибка

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
