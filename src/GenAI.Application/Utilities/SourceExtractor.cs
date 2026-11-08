using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Application.Utilities
{
    public class SourceExtractor
    {
        public static List<string> ExtractSources(string? context)
        {
            if (string.IsNullOrWhiteSpace(context))
            {
                return new List<string>();
            }

            return context
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(entry => entry
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault(line =>
                        line.StartsWith(
                            "Source: ",
                            StringComparison.OrdinalIgnoreCase))
                    ?.Substring("Source: ".Length)
                    ?.Trim())
                .OfType<string>()
                .Where(source => !string.IsNullOrWhiteSpace(source))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
