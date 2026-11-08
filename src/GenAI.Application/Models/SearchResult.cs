using GenAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Application.Models
{
    public class SearchResult
    {
        public DocumentChunk Chunk { get; init; } = new();

        public double Score { get; init; }
    }
}
