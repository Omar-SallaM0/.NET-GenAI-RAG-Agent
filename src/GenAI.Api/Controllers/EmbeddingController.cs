using Microsoft.AspNetCore.Mvc;
using GenAI.Application.Interfaces;
namespace GenAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmbeddingController :ControllerBase
    {
        private readonly IEmbeddingService _embeddingService;

        public EmbeddingController(IEmbeddingService embeddingService)
        {
            _embeddingService = embeddingService;
        }

        [HttpPost]
        public async Task<IActionResult> Generate(
            [FromBody] EmbeddingRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest("Text cannot be empty.");
            }

            var embedding = await _embeddingService
                .GetEmbeddingAsync(request.Text);

            return Ok(new
            {
                Dimensions = embedding.Length,
                Embedding = embedding
            });
        }
        
    }
    public record EmbeddingRequest(string Text);
}
