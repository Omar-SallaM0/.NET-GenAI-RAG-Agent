using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenAI.Infrastructure.Similarity;
namespace GenAI.UnitTests
{
    public class CosineSimilarityServiceTests
    {
        [Fact]
        public void CalculateCosineSimilarity_IdenticalVectors_ReturnsOne()
        {
            // Arrange
            var service = new CosineSimilarityService();
            float[] vectorA = [1, 2, 3];
            float[] vectorB = [1, 2, 3];

            // Act
            var result = service.CalculateCosineSimilarity(vectorA, vectorB);

            // Assert
            Assert.Equal(1.0, result, 10);
        }

        [Fact]
        public void CalculateCosineSimilarity_OrthogonalVectors_ReturnsZero()
        {
            // Arrange
            var service = new CosineSimilarityService();
            float[] vectorA = [1, 0];
            float[] vectorB = [0, 1];

            // Act
            var result = service.CalculateCosineSimilarity(vectorA, vectorB);

            // Assert
            Assert.Equal(0.0, result, 10);
        }

        [Fact]
        public void CalculateCosineSimilarity_DifferentLengthVectors_ThrowsException()
        {
            // Arrange
            var service = new CosineSimilarityService();
            float[] vectorA = [1, 2];
            float[] vectorB = [1, 2, 3];

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                service.CalculateCosineSimilarity(vectorA, vectorB));
        }
    }
}
