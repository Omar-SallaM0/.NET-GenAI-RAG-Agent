using GenAI.Application.Interfaces;
using GenAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenAI.Application.Models;

namespace GenAI.Infrastructure.Search
{
    public class SemanticSearchService : ISemanticSearchService
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IDocumentChunkStore _chunkStore;
        private readonly ISimilarityService _similarityService;
        public SemanticSearchService(
        IEmbeddingService embeddingService,
        IDocumentChunkStore chunkStore,
        ISimilarityService similarityService)
        {
            _embeddingService = embeddingService;
            _chunkStore = chunkStore;
            _similarityService = similarityService;
        }

        public async Task<IReadOnlyList<SearchResult>> SearchAsync(
            string query,
            int topK = 3)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException(
                    "Search query cannot be empty.",
                    nameof(query));
            }

            if (topK <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(topK),
                    "Top K must be greater than zero.");
            }

            var queryEmbedding = await _embeddingService
                .GetEmbeddingAsync(query);

            var chunks = await _chunkStore.GetAllChunksAsync();

            return chunks
                .Select(chunk => new
                {
                    Chunk = chunk,
                    Score = _similarityService.CalculateCosineSimilarity(
                        queryEmbedding,
                        chunk.Embedding)
                })
                .OrderByDescending(item => item.Score)
                .Take(topK)
                .Select(item => new SearchResult
                {
                    Chunk = item.Chunk,
                    Score = item.Score
                })
                //.Select(item => item.Chunk)
                .ToList();
        }
    }
}
