namespace Help.Search.Heroku.Api.Repository
{
    public interface ITokenRepository
    {
        Task<string> GenerateTokenAsync(string username);
    }
}
