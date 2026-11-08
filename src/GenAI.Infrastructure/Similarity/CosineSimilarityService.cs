using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenAI.Application.Interfaces;


namespace GenAI.Infrastructure.Similarity
{
    public class CosineSimilarityService : ISimilarityService
    {
        public double CalculateCosineSimilarity(float[] vectorA, float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
            {
                throw new ArgumentException(
                    "Vectors must have the same length.");
            }

            double dotProduct = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];

                magnitudeA += vectorA[i] * vectorA[i];
                magnitudeB += vectorB[i] * vectorB[i];
            }

            if (magnitudeA == 0 || magnitudeB == 0)
            {
                return 0;
            }

            return dotProduct /
                   (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
        }
    }
}
