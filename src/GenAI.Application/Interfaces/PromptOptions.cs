using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Application.Interfaces
{
    public class PromptOptions
    {
        public string Role { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string OutputFormat { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;
    }
}
