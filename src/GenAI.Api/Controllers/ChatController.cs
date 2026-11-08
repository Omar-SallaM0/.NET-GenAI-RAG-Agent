using Microsoft.AspNetCore.Mvc;
using GenAI.Application.Interfaces;
using GenAI.Application.Utilities;
namespace GenAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IContextProvider _contextProvider;
        public ChatController(IChatService chatService , IContextProvider contextProvider)
        {
            _chatService = chatService;
            _contextProvider = contextProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            var context = await _contextProvider.GetContextAsync(request.Message);

            var response = await _chatService.GetResponseAsync(
                request.Message,
                context);

            //var sources = ExtractSources(context);
            var sources = SourceExtractor.ExtractSources(context);
            return Ok(new
            {
                response,
                sources
            });
        }

       // private static List<string> ExtractSources(string? context)
       // {
       //     if (string.IsNullOrWhiteSpace(context))
       //     {
       //         return new List<string>();
       //     }
       //     return context
       //.Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
       //.Select(entry => entry
       //    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
       //    .FirstOrDefault(line =>
       //        line.StartsWith(
       //            "Source: ",
       //            StringComparison.OrdinalIgnoreCase))
       //    ?.Substring("Source: ".Length)
       //    ?.Trim())
       //.OfType<string>()
       //.Where(source => !string.IsNullOrWhiteSpace(source))
       //.Distinct(StringComparer.OrdinalIgnoreCase)
       //.ToList();


       // }
        public record ChatRequest(string Message);
        //public record ChatRequest(string Message, string? Context);

    }
}
