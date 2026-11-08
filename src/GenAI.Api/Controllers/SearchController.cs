using GenAI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISemanticSearchService _searchService;

        public SearchController(ISemanticSearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromBody] SearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var results = await _searchService.SearchAsync(
                request.Query,
                request.TopK);

            return Ok(new
            {
                Query = request.Query,
                ResultsCount = results.Count,
                Results = results.Select(result => new
                {
                    Id = result.Chunk.Id,
                    Text = result.Chunk.Text,
                    Source = result.Chunk.Source,
                    Score = result.Score
                })
            });
        }
    }
    public record SearchRequest(string Query, int TopK = 3);
    }
