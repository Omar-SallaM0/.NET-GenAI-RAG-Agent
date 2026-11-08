using GenAI.Application.Interfaces;
using GenAI.Infrastructure.AI;
using GenAI.Infrastructure.Context;
using GenAI.Infrastructure.Documents;
using GenAI.Infrastructure.Prompting;
using GenAI.Infrastructure.Search;
using GenAI.Infrastructure.Similarity;
using GenAI.Infrastructure.Storage;
using GenAI.Infrastructure.Text;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddScoped<IChatService, MockChatService>();
builder.Services.AddScoped<IChatService>(sp =>
{

    //var promptOptions = configuration
    //    .GetSection("OpenAI:SystemPrompt")
    //    .Get<PromptOptions>()
    //    ?? throw new InvalidOperationException("OpenAI prompt configuration is not configured.");
    ////var systemPrompt = configuration["OpenAI:SystemPrompt"]
    ////?? throw new InvalidOperationException("OpenAI system prompt is not configured.");

    //return new OpenAIChatService(apiKey, promptOptions);
    var configuration = sp.GetRequiredService<IConfiguration>();

    var apiKey = configuration["OpenAI:ApiKey"]
        ?? throw new InvalidOperationException("OpenAI API key is not configured.");

    var promptBuilder = sp.GetRequiredService<IPromptBuilder>();

    return new OpenAIChatService(apiKey, promptBuilder);
});
builder.Services.AddScoped<IPromptBuilder>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var promptOptions = configuration
        .GetSection("OpenAI:SystemPrompt")
        .Get<PromptOptions>()
        ?? throw new InvalidOperationException(
            "OpenAI prompt configuration is not configured.");

    return new PromptBuilder(promptOptions);
});

builder.Services.AddScoped<IContextProvider, SimpleContextProvider>();
builder.Services.AddScoped<ITextChunker, SimpleTextChunker>();
builder.Services.AddScoped<IEmbeddingService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var apiKey = configuration["OpenAI:ApiKey"]
        ?? throw new InvalidOperationException("OpenAI API key is not configured.");
    return new OpenAIEmbeddingService(apiKey);
});
builder.Services.AddScoped<ISimilarityService, CosineSimilarityService>();
builder.Services.AddSingleton<IDocumentChunkStore, InMemoryDocumentChunkStore>();
builder.Services.AddScoped<IDocumentIngestionService, DocumentIngestionService>();
builder.Services.AddScoped<ISemanticSearchService, SemanticSearchService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
