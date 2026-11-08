using GenAI.Application.Interfaces;
using GenAI.Domain.Entities;
using GenAI.Infrastructure.Documents;

using Xunit;

namespace GenAI.UnitTests
{
    public class DocumentIngestionServiceTests
    {
        [Fact]
        public async Task IngestAsync_StoresSourceWithEachChunk()
        {
            // Arrange
            var chunker = new FakeTextChunker();
            var embeddingService = new FakeEmbeddingService();
            var chunkStore = new FakeDocumentChunkStore();

            var service = new DocumentIngestionService(
                chunker,
                embeddingService,
                chunkStore);

            // Act
            var count = await service.IngestAsync(
                "First chunk. Second chunk.",
                "sample.txt");

            // Assert
            Assert.Equal(2, count);
            Assert.Equal(2, chunkStore.Chunks.Count);
            Assert.All(
                chunkStore.Chunks,
                chunk => Assert.Equal("sample.txt", chunk.Source));
        }

        private sealed class FakeTextChunker : ITextChunker
        {
            public IReadOnlyList<string> ChunkText(string text)
            {
                return new[] { "First chunk.", "Second chunk." };
            }
        }

        private sealed class FakeEmbeddingService : IEmbeddingService
        {
            public Task<float[]> GetEmbeddingAsync(string text)
            {
                return Task.FromResult(new float[] { 0.1f, 0.2f });
            }
        }

        private sealed class FakeDocumentChunkStore : IDocumentChunkStore
        {
            public List<DocumentChunk> Chunks { get; } = new();

            public Task AddChunksAsync(
                IReadOnlyList<DocumentChunk> chunks)
            {
                Chunks.AddRange(chunks);
                return Task.CompletedTask;
            }

            public Task<IReadOnlyList<DocumentChunk>> GetAllChunksAsync()
            {
                return Task.FromResult<IReadOnlyList<DocumentChunk>>(Chunks);
            }
        }
    }
}
