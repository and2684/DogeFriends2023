using DogeFriendsSharedClassLibrary.Dto;

namespace DogeFriendsApi.Interfaces
{
    public interface ITokenService
    {
        public Task<bool> ValidateAccessTokenAsync(string accessToken);
        public Task<(string accessToken, string refreshToken)> RefreshTokensAsync(TokensDto tokensDto);
    }
}
