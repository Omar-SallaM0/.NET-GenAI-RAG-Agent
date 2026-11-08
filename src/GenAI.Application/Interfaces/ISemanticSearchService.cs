using GenAI.Application.Models;
using GenAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Application.Interfaces
{
    public interface ISemanticSearchService
    {
        Task<IReadOnlyList<SearchResult>> SearchAsync(string query, int topK = 3);
    }
}
