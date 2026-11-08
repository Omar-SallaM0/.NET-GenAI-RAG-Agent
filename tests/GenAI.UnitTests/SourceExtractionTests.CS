using GenAI.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.UnitTests
{
    public class SourceExtractionTests
    {
      
            [Fact]
            public void ExtractSources_NullContext_ReturnsEmptyList()
            {
                // Arrange
                string? context = null;

                // Act
                var result = SourceExtractor.ExtractSources(context);

                // Assert
                Assert.Empty(result);
            }

            [Fact]
            public void ExtractSources_EmptyContext_ReturnsEmptyList()
            {
                // Arrange
                var context = string.Empty;

                // Act
                var result = SourceExtractor.ExtractSources(context);

                // Assert
                Assert.Empty(result);
            }

            [Fact]
            public void ExtractSources_NoSources_ReturnsEmptyList()
            {
                // Arrange
                var context = "This is a sample AI response.";

                // Act
                var result = SourceExtractor.ExtractSources(context);

                // Assert
                Assert.Empty(result);
            }

            [Fact]
            public void ExtractSources_MultipleSources_ReturnsAllSources()
            {
                // Arrange
                var context =
                    "First paragraph.\n" +
                    "Source: document1.pdf\n\n" +
                    "Second paragraph.\n" +
                    "Source: document2.pdf";

                // Act
                var result = SourceExtractor.ExtractSources(context);

                // Assert
                Assert.Equal(2, result.Count);
                Assert.Equal("document1.pdf", result[0]);
                Assert.Equal("document2.pdf", result[1]);
            }

            [Fact]
            public void ExtractSources_DuplicateSources_ReturnsUniqueSources()
            {
                // Arrange
                var context =
                    "Source: document1.pdf\n\n" +
                    "Source: document2.pdf\n\n" +
                    "Source: DOCUMENT1.PDF";

                // Act
                var result = SourceExtractor.ExtractSources(context);

                // Assert
                Assert.Equal(2, result.Count);
                Assert.Equal("document1.pdf", result[0]);
                Assert.Equal("document2.pdf", result[1]);
            }
        
    }
}

