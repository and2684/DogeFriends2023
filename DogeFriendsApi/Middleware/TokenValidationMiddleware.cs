using DogeFriendsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace DogeFriendsApi.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
        {
            if (context.GetEndpoint()?.Metadata.GetMetadata<IAuthorizeData>() == null)
            {
                await _next(context);
                return;
            }

            var accessToken = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(accessToken))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Токен не найден.");
                return;
            }

            accessToken = accessToken.Replace("Bearer ", string.Empty);

            var isValidToken = await tokenService.ValidateAccessTokenAsync(accessToken);

            if (!isValidToken)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Токен не прошел валидацию.");
                return;
            }

            await _next(context);
        }
    }
}