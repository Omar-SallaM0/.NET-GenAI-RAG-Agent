using GenAI.Infrastructure.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace GenAI.UnitTests
{
    public class SimpleTextChunkerTests
    {
        [Fact]
        public void Chunk_EmptyText_ReturnsNoChunks()
        {
            // Arrange
            var chunker = new SimpleTextChunker();

            // Act
            var result = chunker.ChunkText(string.Empty);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Chunk_TextOf600Characters_ReturnsTwoChunksWithOverlap()
        {
            // Arrange
            var chunker = new SimpleTextChunker();
            var text = new string('A', 600);

            // Act
            var result = chunker.ChunkText(text);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(500, result[0].Length);
            Assert.Equal(150, result[1].Length);
            Assert.Equal(result[0][450..], result[1][..50]);
        }
    }
}
