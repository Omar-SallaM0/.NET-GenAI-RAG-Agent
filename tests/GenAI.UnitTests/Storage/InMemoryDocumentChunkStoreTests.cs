using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenAI.Domain.Entities;
using GenAI.Infrastructure.Storage;

namespace GenAI.UnitTests.Storage
{

    public class InMemoryDocumentChunkStoreTests
    {
        [Fact]
        public async Task AddChunksAsync_ShouldStoreDocumentChunks()
        {
            // Arrange
            var store = new InMemoryDocumentChunkStore();

            var chunks = new List<DocumentChunk>
            {
                new()
                {
                    Text = "Forex is the foreign exchange market.",
                    Source = "Forex_Guide.txt"
                },
                new()
                {
                    Text = "Currency pairs are traded in forex.",
                    Source = "Forex_Guide.txt"
                }
            };

            // Act
            await store.AddChunksAsync(chunks);

            var result = await store.GetAllChunksAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, chunk =>
                Assert.Equal("Forex_Guide.txt", chunk.Source));
        }

        [Fact]
        public async Task AddChunksAsync_ShouldReplaceExistingChunks_WhenSameSourceIsAddedAgain()
        {
            // Arrange
            var store = new InMemoryDocumentChunkStore();

            var firstIngestion = new List<DocumentChunk>
    {
        new()
        {
            Text = "Old Forex content",
            Source = "Forex_Guide.txt"
        }
    };

            var secondIngestion = new List<DocumentChunk>
    {
        new()
        {
            Text = "Updated Forex content",
            Source = "Forex_Guide.txt"
        },
        new()
        {
            Text = "Additional Forex content",
            Source = "Forex_Guide.txt"
        }
    };

            // Act
            await store.AddChunksAsync(firstIngestion);
            await store.AddChunksAsync(secondIngestion);

            var result = await store.GetAllChunksAsync();

            // Assert
            Assert.Equal(2, result.Count);

            Assert.DoesNotContain(result, chunk =>
                chunk.Text == "Old Forex content");

            Assert.Contains(result, chunk =>
                chunk.Text == "Updated Forex content");

            Assert.Contains(result, chunk =>
                chunk.Text == "Additional Forex content");
        }

        [Fact]
        public async Task AddChunksAsync_ShouldPreserveOtherDocuments_WhenSameSourceIsReingested()
        {
            // Arrange
            var store = new InMemoryDocumentChunkStore();

            var forexChunks = new List<DocumentChunk>
    {
        new()
        {
            Text = "Forex original content",
            Source = "Forex_Guide.txt"
        }
    };

            var bankingChunks = new List<DocumentChunk>
    {
        new()
        {
            Text = "Banking information",
            Source = "Banking_Guide.txt"
        }
    };

            var updatedForexChunks = new List<DocumentChunk>
    {
        new()
        {
            Text = "Updated Forex content",
            Source = "Forex_Guide.txt"
        }
    };

            // Act
            await store.AddChunksAsync(forexChunks);
            await store.AddChunksAsync(bankingChunks);
            await store.AddChunksAsync(updatedForexChunks);

            var result = await store.GetAllChunksAsync();

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Contains(result, chunk =>
                chunk.Source == "Forex_Guide.txt" &&
                chunk.Text == "Updated Forex content");

            Assert.Contains(result, chunk =>
                chunk.Source == "Banking_Guide.txt" &&
                chunk.Text == "Banking information");

            Assert.DoesNotContain(result, chunk =>
                chunk.Text == "Forex original content");
        }

        [Fact]
        public async Task AddChunksAsync_ShouldNotChangeExistingData_WhenEmptyListIsAdded()
        {
            // Arrange
            var store = new InMemoryDocumentChunkStore();

            var existingChunks = new List<DocumentChunk>
    {
        new()
        {
            Text = "Forex information",
            Source = "Forex_Guide.txt"
        }
    };

            // Act
            await store.AddChunksAsync(existingChunks);

            await store.AddChunksAsync(new List<DocumentChunk>());

            var result = await store.GetAllChunksAsync();

            // Assert
            Assert.Single(result);

            Assert.Equal("Forex_Guide.txt", result[0].Source);
            Assert.Equal("Forex information", result[0].Text);
        }

        [Fact]
        public async Task AddChunksAsync_ShouldReplaceExistingChunks_WhenSourceHasDifferentCasing()
        {
            // Arrange
            var store = new InMemoryDocumentChunkStore();

            var originalChunks = new List<DocumentChunk>
    {
        new()
        {
            Text = "Original Forex content",
            Source = "Forex_Guide.txt"
        }
    };

            var updatedChunks = new List<DocumentChunk>
    {
        new()
        {
            Text = "Updated Forex content",
            Source = "forex_guide.txt"
        }
    };

            // Act
            await store.AddChunksAsync(originalChunks);
            await store.AddChunksAsync(updatedChunks);

            var result = await store.GetAllChunksAsync();

            // Assert
            Assert.Single(result);

            Assert.Equal("Updated Forex content", result[0].Text);
            Assert.Equal("forex_guide.txt", result[0].Source);
        }
    }
}
