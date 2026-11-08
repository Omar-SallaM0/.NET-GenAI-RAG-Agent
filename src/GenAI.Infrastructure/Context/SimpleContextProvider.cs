using GenAI.Application.Interfaces;
using GenAI.Infrastructure.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Infrastructure.Context
{
    public class SimpleContextProvider : IContextProvider
    {
        private readonly ISemanticSearchService _semanticSearchService;
        public SimpleContextProvider(
            ISemanticSearchService semanticSearchService)
        {
            _semanticSearchService = semanticSearchService;
        }

        public async Task<string> GetContextAsync(string query)
        {
            var results = await _semanticSearchService.SearchAsync(
                query,
                topK: 3);

            var context = string.Join(
                "\n\n",
                results.Select(
                    result => /*result.Chunk.Text*/
                    $"Source: {result.Chunk.Source}\n" +
                   $"Content: {result.Chunk.Text}"
                    ));

            return context;
        }
        //public Task<string> GetContextAsync(string query)
        //{
        //    //throw new NotImplementedException();
        //    var context = """
        //This application is built using ASP.NET Core and follows
        //dependency injection and clean architecture principles.
        //The application uses interfaces to keep components loosely coupled.
        //""";

        //    return Task.FromResult(context);
        //}
    }
}
