using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenAI.Application.Interfaces;
using OpenAI.Realtime;
namespace GenAI.Infrastructure.AI
{
    public class MockChatService : IChatService
    {
        public  Task<string> GetResponseAsync(string message, string? Context)
        {
            var response =
            $"Mock AI Response: I received your message '{message}'.";

            // Simulate a delay to mimic an actual API call
           // await Task.Delay(500);
            // Return a mock response based on the input message
            //return $"Mock response to: {message}";
            return Task.FromResult(response);
        }
    }
}
