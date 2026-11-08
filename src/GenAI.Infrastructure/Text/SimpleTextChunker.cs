using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenAI.Application.Interfaces;
namespace GenAI.Infrastructure.Text
{
    public class SimpleTextChunker: ITextChunker
    {
        public IReadOnlyList<string> ChunkText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<string>();
            }

            const int chunkSize = 500;
            const int overlap = 50;

            var chunks = new List<string>();

            for (int start = 0; start < text.Length; start += chunkSize - overlap)
            {
                var length = Math.Min(chunkSize, text.Length - start);
                var chunk = text.Substring(start, length).Trim();

                if (!string.IsNullOrWhiteSpace(chunk))
                {
                    chunks.Add(chunk);
                }
            }

            return chunks;
        }

        
    }
}
