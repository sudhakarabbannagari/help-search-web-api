using Help.Search.Heroku.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Help.Search.Heroku.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class HelpSearchController : ControllerBase
    {

        private readonly ILogger<HelpSearchController> _logger;
        private readonly ITokenRepository _tokenReposity;
        private readonly IHelpSearchRepository _searchRepository;

        public HelpSearchController(ILogger<HelpSearchController> logger, ITokenRepository tokenReposity, IHelpSearchRepository searchRepository)
        {
            _logger = logger;
            this._tokenReposity = tokenReposity;
            this._searchRepository = searchRepository;
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                _logger.LogWarning($"Quetry string is empty for {nameof(query)}");
                return BadRequest("Query parameter is required.");
            }

            var result = await this._searchRepository.SearchAsync(query);

            _logger.LogInformation("Successfully retrieved the search result");
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("GenerateToken/{userName}")]
        public async Task<IActionResult> GenerateToken(string userName)
        {
            if (!string.IsNullOrEmpty(userName) && userName == "admin")
            {
                var token = await _tokenReposity.GenerateTokenAsync(userName);
                _logger.LogInformation("Token generated successfully.");
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }


    }

}
