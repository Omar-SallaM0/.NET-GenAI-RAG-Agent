# DotNet GenAI RAG Agent
 
A Retrieval-Augmented Generation (RAG) chatbot built with **ASP.NET Core** and **OpenAI**, structured with **Clean Architecture**. Ingest your own documents, search them semantically, and chat with an AI assistant that answers from your content and cites its sources.
 
---
 
## Features
 
- **RAG Chat API**: ask a question, get an answer grounded in your documents, plus the list of sources used.
- **Document Ingestion**: send text with a source name; it is chunked, embedded, and stored.
- **Semantic Search**: meaning-based search using embeddings and cosine similarity (Top-K, default `3`).
- **Source Citations**: every chunk keeps its source name; the prompt forces the model to cite exact sources and never invent them.
- **Configurable System Prompt**: change role, level, language, output format, and instructions from `appsettings.json` without touching code.
- **Utility Endpoints**: Embedding and Similarity controllers for experimenting with embeddings and cosine similarity directly.
- **Mock Chat Service**: develop and test without an OpenAI API key.
- **Swagger UI** in Development.
- **Unit Tests** for the chunker, similarity, ingestion, semantic search, context provider, source extraction, and the in-memory store.
---
 
## Architecture
 
```
src/
├── GenAI.Api              # Web API: Controllers, Program.cs, configuration
├── GenAI.Application      # Interfaces, models, utilities (contracts only)
├── GenAI.Domain           # Entities (DocumentChunk)
└── GenAI.Infrastructure   # Implementations
    ├── AI                 # OpenAIChatService, OpenAIEmbeddingService, MockChatService
    ├── Context            # SimpleContextProvider
    ├── Documents          # DocumentIngestionService
    ├── Prompting          # PromptBuilder
    ├── Search             # SemanticSearchService
    ├── Similarity         # CosineSimilarityService
    ├── Storage            # InMemoryDocumentChunkStore
    └── Text               # SimpleTextChunker
tests/
└── GenAI.UnitTests
```
 
Dependencies point inward: `Api → Infrastructure → Application → Domain`.
 
### How the RAG flow works
 
```
POST /api/chat { "message": "..." }
        │
        ▼
 IContextProvider ──► embed the question
        │             compare with all stored chunks (cosine similarity)
        │             take top 3 chunks
        ▼
 Context = "Source: <name>\nContent: <chunk text>" (entries separated by a blank line)
        │
        ▼
 PromptBuilder ──► system prompt (persona + RAG rules + context)
        │
        ▼
 OpenAI Responses API ──► answer
        │
        ▼
 { "response": "...", "sources": ["..."] }
```
 
---
 
## Getting Started
 
### Prerequisites
 
- [.NET SDK](https://dotnet.microsoft.com/download) (matching the version in the `.csproj` files)
- An [OpenAI API key](https://platform.openai.com/api-keys)
### Setup
 
```bash
git clone https://github.com/Rohit-1480/DotNet-GenAI-RAG-Agent.git
cd DotNet-GenAI-RAG-Agent
```
 
Store your API key securely (do **not** commit it):
 
```bash
cd src/GenAI.Api
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "YOUR_API_KEY"
```
 
Or use an environment variable:
 
```bash
export OpenAI__ApiKey="YOUR_API_KEY"
```
 
### Configure the prompt
 
In `appsettings.json`:
 
```json
{
  "OpenAI": {
    "SystemPrompt": {
      "Role": "helpful technical assistant",
      "Level": "beginner",
      "Language": "C#",
      "OutputFormat": "short paragraphs",
      "Instructions": "Be concise and accurate."
    }
  }
}
```
 
### Run
 
```bash
dotnet run --project src/GenAI.Api
```
 
Open Swagger UI at `https://localhost:<port>/swagger` (Development only).
 
### Run the tests
 
```bash
dotnet test
```
 
---
 
## API
 
### Ingest a document
 
`POST /api/document/ingest`
 
```json
{
  "text": "ASP.NET Core supports dependency injection out of the box...",
  "source": "aspnet-notes.txt"
}
```
 
Response:
 
```json
{
  "message": "Document ingested successfully.",
  "source": "aspnet-notes.txt",
  "chunksCreated": 1
}
```
 
Returns `400 Bad Request` if `text` or `source` is empty.
 
### Chat
 
`POST /api/chat`
 
```json
{ "message": "Does ASP.NET Core support dependency injection?" }
```
 
Response:
 
```json
{
  "response": "Yes. According to aspnet-notes.txt, ...",
  "sources": ["aspnet-notes.txt"]
}
```
 
### Other endpoints
 
The `Search`, `Embedding`, and `Similarity` controllers expose the underlying building blocks for experimentation. Check Swagger UI for their exact routes and request shapes.
 
---
 
## Implementation Notes
 
| Area | Behavior |
|------|----------|
| Chunking | Fixed-size by characters: **500 chars** per chunk, **50 chars** overlap |
| Retrieval | Brute-force cosine similarity over all stored chunks, Top-K = 3 |
| Storage | In-memory, registered as a **Singleton** (data is lost on restart) |
| Chat model | OpenAI Responses API with `gpt-5.2` (hardcoded in `OpenAIChatService`) |
| Mock service | Switch to `MockChatService` by editing the `IChatService` registration in `Program.cs` |
 
---
 
## Known Limitations
 
- **In-memory storage**: documents must be re-ingested after every restart; no real vector database.
- **Brute-force search**: similarity is computed against every chunk, which slows down with a large number of chunks.
- **No similarity threshold**: the top 3 chunks are always returned, even if unrelated, so `sources` may include irrelevant entries.
- **Sources come from retrieved context**, not from what the model actually used in its answer.
- **Sequential embeddings** during ingestion (one request per chunk).
- **No duplicate protection**: ingesting the same document twice duplicates its chunks.
- **No authentication or rate limiting** on the API.
- **No conversation history**: each message is independent.
- **Limited error handling and validation** on `/api/chat` (an empty message results in a 500 instead of a 400).
- **Prompt injection risk**: document content is placed directly in the system prompt.
- Not a full "agent": there are no tools, planning, or memory, so it is a RAG chatbot.
---
 
## Tech Stack
 
- C# / ASP.NET Core Web API
- OpenAI .NET SDK (Responses API and Embeddings)
- Swagger / Swashbuckle
- xUnit-style unit tests
---
 ## 👨‍💻 Author

**Omar Ahmed Sallam**

AI Full-Stack (.NET & Angular) Developer

* GitHub: [Omar-SallaM0](https://github.com/Omar-SallaM0)
* LinkedIn: [Omar Sallam](https://www.linkedin.com/in/omar-sallam-9aa483259/)
