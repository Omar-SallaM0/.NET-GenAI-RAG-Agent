using GenAI.Application.Interfaces;
using GenAI.Domain.Entities;
using GenAI.Application.Models;
using GenAI.Infrastructure.Context;
using Xunit;

namespace GenAI.UnitTests
{
   
    public class SimpleContextProviderTests
    {
        [Fact]
        public async Task GetContextAsync_ReturnsTextFromSearchResults()
        {
            // Arrange
            var chunks = new List<SearchResult>
        {
            new SearchResult
            {
                Chunk = new DocumentChunk
                {
                    Text = "Dependency injection improves testability."
                },
                Score = 0.9
            },
            new SearchResult
            {
                Chunk = new DocumentChunk
                {
                    Text = "Semantic search uses embeddings."
                },
                Score = 0.8
            }
        };

            var fakeSearchService = new FakeSemanticSearchService(chunks);
            var contextProvider = new SimpleContextProvider(fakeSearchService);

            // Act
            var context = await contextProvider.GetContextAsync("semantic search");

            // Assert
            Assert.Contains("Dependency injection improves testability.", context);
            Assert.Contains("Semantic search uses embeddings.", context);
            Assert.Equal(1, fakeSearchService.CallCount);
            Assert.Equal("semantic search", fakeSearchService.LastQuery);
            Assert.Equal(3, fakeSearchService.LastTopK);
        }

        private sealed class FakeSemanticSearchService : ISemanticSearchService
        {
            private readonly IReadOnlyList<SearchResult> _results;

            public int CallCount { get; private set; }
            public string? LastQuery { get; private set; }
            public int LastTopK { get; private set; }

            public FakeSemanticSearchService(IReadOnlyList<SearchResult> results)
            {
                _results = results;
            }

            public Task<IReadOnlyList<SearchResult>> SearchAsync(
                string query, int topK = 3)
            {
                CallCount++;
                LastQuery = query;
                LastTopK = topK;

                return Task.FromResult(_results);
            }
        }
    }
}
