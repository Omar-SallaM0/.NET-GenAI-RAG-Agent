using GenAI.Application.Interfaces;

using OpenAI.Embeddings;


namespace GenAI.Infrastructure.AI
{
    public class OpenAIEmbeddingService : IEmbeddingService
    {
        private readonly EmbeddingClient _client;

        public OpenAIEmbeddingService(string apiKey)
        {
            _client = new EmbeddingClient(
                "text-embedding-3-small",
                apiKey);
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Text cannot be empty.",
                    nameof(text));
            }

            var response = await _client.GenerateEmbeddingAsync(text);

            return response.Value.ToFloats().ToArray();
        }

    }
}
