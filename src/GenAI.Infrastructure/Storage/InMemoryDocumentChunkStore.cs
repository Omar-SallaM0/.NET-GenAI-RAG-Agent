using GenAI.Application.Interfaces;
using GenAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Infrastructure.Storage
{
    public class InMemoryDocumentChunkStore : IDocumentChunkStore
    {
        private readonly List<DocumentChunk> _chunks = new();
        public Task AddChunksAsync(IReadOnlyList<DocumentChunk> chunks)
        {
            //_chunks.AddRange(chunks);
            //return Task.CompletedTask;
            if (chunks == null || chunks.Count == 0)
                return Task.CompletedTask;

            var source = chunks[0].Source;

            // Remove existing chunks belonging to the same source
            _chunks.RemoveAll(x =>
                string.Equals(
                    x.Source,
                    source,
                    StringComparison.OrdinalIgnoreCase));

            // Add the latest document chunks
            _chunks.AddRange(chunks);

            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<DocumentChunk>> GetAllChunksAsync()
        {
            IReadOnlyList<DocumentChunk> result = _chunks.ToList();
            return Task.FromResult(result);
        }
    }
}
