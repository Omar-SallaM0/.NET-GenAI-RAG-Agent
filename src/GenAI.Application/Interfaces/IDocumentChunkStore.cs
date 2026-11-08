using GenAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Application.Interfaces
{
    public interface IDocumentChunkStore
    {
        Task AddChunksAsync(IReadOnlyList<DocumentChunk> chunks);

        Task<IReadOnlyList<DocumentChunk>> GetAllChunksAsync();
    }
}
