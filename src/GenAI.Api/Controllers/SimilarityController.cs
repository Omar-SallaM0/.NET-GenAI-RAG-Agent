using GenAI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimilarityController : ControllerBase
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly ISimilarityService _similarityService;

        public SimilarityController(
            IEmbeddingService embeddingService,
            ISimilarityService similarityService)
        {
            _embeddingService = embeddingService;
            _similarityService = similarityService;
        }

        [HttpPost]
        public async Task<IActionResult> Compare(
            [FromBody] SimilarityRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text1) ||
                string.IsNullOrWhiteSpace(request.Text2))
            {
                return BadRequest("Both texts are required.");
            }

            var embedding1 = await _embeddingService
                .GetEmbeddingAsync(request.Text1);

            var embedding2 = await _embeddingService
                .GetEmbeddingAsync(request.Text2);

            var similarity = _similarityService
                .CalculateCosineSimilarity(embedding1, embedding2);

            return Ok(new
            {
                Text1 = request.Text1,
                Text2 = request.Text2,
                SimilarityScore = Math.Round(similarity, 4)
            });
        }
    }
}
