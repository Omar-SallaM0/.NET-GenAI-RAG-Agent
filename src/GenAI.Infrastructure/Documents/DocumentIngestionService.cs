using GenAI.Application.Interfaces;
using GenAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Infrastructure.Documents
{
    public class DocumentIngestionService : IDocumentIngestionService
    {
        private readonly ITextChunker _textChunker;
        private readonly IEmbeddingService _embeddingService;
        private readonly IDocumentChunkStore _chunkStore;
        public DocumentIngestionService(ITextChunker textChunker, IEmbeddingService embeddingService, IDocumentChunkStore chunkStore)
        {
            _textChunker = textChunker;
            _embeddingService = embeddingService;
            _chunkStore = chunkStore;
        }

        public async Task<int> IngestAsync(string documentText, string source)
        {
            if (string.IsNullOrWhiteSpace(documentText))
            {
                throw new ArgumentException(
                    "Document text cannot be empty.",
                    nameof(documentText));
            }
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException(
                    "Source cannot be empty.",
                    nameof(source));
            }
            var textChunks = _textChunker.ChunkText(documentText);
            var documentChunks = new List<DocumentChunk>();
            foreach (var text in textChunks)
            {
                var embedding = await _embeddingService.GetEmbeddingAsync(text);

                documentChunks.Add(new DocumentChunk
                {
                    Text = text,
                    Embedding = embedding,
                    Source = source
                });
            }

            await _chunkStore.AddChunksAsync(documentChunks);

            return documentChunks.Count;
        }
    }
}
