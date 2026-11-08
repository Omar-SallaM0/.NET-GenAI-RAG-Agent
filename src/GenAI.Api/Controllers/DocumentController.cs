using GenAI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenAI.Api.Controllers
{
    [ApiController]
   [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentIngestionService _ingestionService;

        public DocumentController(IDocumentIngestionService ingestionService)
        {
            _ingestionService = ingestionService;
        }

        [HttpPost("ingest")]
        public async Task<IActionResult> Ingest(
        [FromBody] IngestDocumentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest("Document text cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(request.Source))
            {
                return BadRequest("Document source cannot be empty.");
            }
            var chunkCount = await _ingestionService
                .IngestAsync(request.Text, request.Source);

            return Ok(new
            {
                Message = "Document ingested successfully.",
                Source = request.Source,
                ChunksCreated = chunkCount
            });
        }
    }
    public record IngestDocumentRequest(string Text, string Source);
}
