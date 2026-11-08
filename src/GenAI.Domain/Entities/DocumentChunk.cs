using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Domain.Entities
{
    public class DocumentChunk
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public string Text { get; init; } = string.Empty;

        public float[] Embedding { get; init; } = Array.Empty<float>();
        public string Source { get; init; } = string.Empty;
    }
}
