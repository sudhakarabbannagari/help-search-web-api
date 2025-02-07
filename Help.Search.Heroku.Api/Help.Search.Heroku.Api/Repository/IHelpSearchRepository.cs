using Help.Search.Heroku.Api.DTOs;

namespace Help.Search.Heroku.Api.Repository
{
    public interface IHelpSearchRepository
    {
        Task<SearchResponse> SearchAsyncTest(string query);
        Task<string> SearchAsync(string query);
    }
}
