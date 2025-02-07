using Help.Search.Heroku.Api.Models;

namespace Help.Search.Heroku.Api.DTOs
{
    public class SearchResponse
    {
        public bool Success { get; set; }
        public SearchdData? Data { get; set; }
    }

}
