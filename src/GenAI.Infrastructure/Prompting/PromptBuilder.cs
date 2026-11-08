using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenAI.Application.Interfaces;


namespace GenAI.Infrastructure.Prompting
{
    public class PromptBuilder : IPromptBuilder
    {
        //private readonly PromptOptions _promptOptions;

        //public PromptBuilder(PromptOptions promptOptions)
        //{
        //    _promptOptions = promptOptions;
        //}
        //public string BuildSystemPrompt()
        //{
        //    return
        //   $"You are a {_promptOptions.Role}. " +
        //   $"Explain concepts at a {_promptOptions.Level} level. " +
        //   $"Use {_promptOptions.Language} examples when appropriate."+
        //   $"Format your response as {_promptOptions.OutputFormat}."+
        //   $"Follow these instructions: {_promptOptions.Instructions}";
        //}
        private readonly PromptOptions _promptOptions;

        private const string SystemPromptTemplate = """
                                                  You are a {Role}.

                                               Explain concepts at a {Level} level.

                                              Use {Language} examples when appropriate.

                                             Format your response as {OutputFormat}.

                                                 Follow these instructions:
                                         {Instructions}
                                         Rules for answering using document context:
1. Use the provided context when relevant.
2. Each context entry contains a Source and Content.
3. When using information from the context, cite the exact source name.
4. Never invent source names or citations.
5. If the context lacks sufficient information, say so.

Additional context:
{Context}
""";

        public PromptBuilder(PromptOptions promptOptions)
        {
            _promptOptions = promptOptions;
        }

        public string BuildSystemPrompt(string? context)
        {
            return SystemPromptTemplate
                .Replace("{Role}", _promptOptions.Role)
                .Replace("{Level}", _promptOptions.Level)
                .Replace("{Language}", _promptOptions.Language)
                .Replace("{OutputFormat}", _promptOptions.OutputFormat)
                .Replace("{Instructions}", _promptOptions.Instructions)
                //.Replace("{Context}", _promptOptions.Context);
                .Replace("{Context}", context ?? string.Empty);
        }
    }
}
