using Help.Search.Heroku.Api.Controllers;
using Help.Search.Heroku.Api.Data;
using Help.Search.Heroku.Api.DTOs;
using Help.Search.Heroku.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace Help.Search.Heroku.Api.Repository
{
    public class HelpSearchRepository : IHelpSearchRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HelpSearchController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public HelpSearchRepository(ApplicationDbContext context, ILogger<HelpSearchController> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }


        public async Task<string> SearchAsync(string query)
        {
            string ApiKey = _configuration["GOOGLE_API_KEY"];
            string SearchEngineId = _configuration["SEARCH_ENGINE_ID"];

            string url = $"https://www.googleapis.com/customsearch/v1?q={Uri.EscapeDataString(query)}&key={ApiKey}&cx={SearchEngineId}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Retrieved the search result for {query} search criteria.");
                return result; // Raw JSON response
            }
            else
            {
                _logger.LogError($"Error: {response.StatusCode}");
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        public async Task<SearchResponse> SearchAsyncTest(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                return await Task.Run(() => new SearchResponse()
                {
                    Success = true,
                    Data = this._context.Data?.Where(x => x.Name == query).FirstOrDefault()
                });
            }
            _logger.LogInformation($"Retrieved the search result for {query} search criteria.");
            return await Task.Run(() => new SearchResponse());
        }
    }
}
