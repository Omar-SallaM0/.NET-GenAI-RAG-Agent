using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Application.Interfaces
{
    public  interface IChatService
    {
      //  Task<string> GetResponseAsync(string message);
        Task<string> GetResponseAsync(string message, string? context);
    }
}
