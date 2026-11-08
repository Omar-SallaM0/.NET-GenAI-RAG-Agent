using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Application.Interfaces
{
    public interface IPromptBuilder
    {
        string BuildSystemPrompt(string? context);
    }
}
