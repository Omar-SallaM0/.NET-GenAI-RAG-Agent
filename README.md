# Enterprise GenAI RAG Application with .NET 9

An AI-powered Retrieval-Augmented Generation (RAG) application built using **C#, .NET 9, ASP.NET Core Web API, and OpenAI**.

This project demonstrates how modern Generative AI capabilities can be implemented using the .NET ecosystem and Clean Architecture principles.

## Key Features

- OpenAI LLM integration
- Configurable system prompts and prompt engineering
- Text embeddings generation
- Document ingestion and text chunking
- Semantic search using cosine similarity
- Multi-document retrieval
- Context-aware AI-generated responses
- Source attribution and citation tracking
- RESTful APIs with Swagger
- Unit testing using xUnit

## Technology Stack

- C#
- .NET 9
- ASP.NET Core Web API
- OpenAI API
- Embeddings
- Retrieval-Augmented Generation (RAG)
- Semantic Search
- Clean Architecture
- Swagger / OpenAPI
- xUnit

## Solution Architecture

- **GenAI.Api** – REST API endpoints
- **GenAI.Application** – Application services and interfaces
- **GenAI.Domain** – Core domain models
- **GenAI.Infrastructure** – OpenAI integration, embeddings, ingestion and retrieval
- **GenAI.UnitTests** – Unit tests

## Current Capabilities

1. Ingest text documents with source information.
2. Split documents into smaller chunks.
3. Generate embeddings for document chunks.
4. Search relevant content using semantic similarity.
5. Provide retrieved context to the LLM.
6. Generate responses with source references.

## Current Project Status

Working prototype with multi-document ingestion, semantic retrieval and source-cited AI responses.

**Current limitation:** Document chunks are stored in memory and are lost when the API process restarts. Persistent vector storage and additional document formats are planned enhancements.

## Future Enhancements

- Persistent vector database integration
- PDF and DOCX document ingestion
- Improved retrieval and ranking
- Conversation memory
- AI tool/function calling
- AI agents and multi-agent workflows
- Azure deployment

## Learning & Development Objective

This project is part of a practical journey covering:

**LLMs → Prompt Engineering → Embeddings → RAG → Tool Calling → AI Agents → Multi-Agent Systems**

The objective is to build practical, scalable GenAI applications using Microsoft .NET technologies.
