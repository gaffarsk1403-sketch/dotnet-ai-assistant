# .NET AI Assistant

A full-stack portfolio project that combines **ASP.NET Core, C#, React, TypeScript, REST APIs, and LLM integration** in a clean, recruiter-friendly architecture.

The application answers questions using a small internal knowledge base, retrieves relevant context, and sends only that context to the model so responses stay grounded.

## What it demonstrates

- ASP.NET Core Web API
- C# service abstractions
- REST API design
- React + TypeScript frontend
- OpenAI API integration
- prompt grounding
- simple retrieval logic
- validation and safe fallbacks
- xUnit tests
- Docker support
- GitHub Actions CI
- Azure-ready architecture

## Architecture

```text
React UI
   |
   v
ASP.NET Core API
   |
   +--> Request Validation
   |
   +--> Knowledge Retrieval
   |
   +--> IAiService
          |
          v
       OpenAI API
   |
   v
Answer + Source Names
```

The AI provider is hidden behind an interface, keeping the web layer separate from model-specific code.

See [docs/architecture.md](docs/architecture.md) for additional design notes.

## Backend

The backend is located at:

```text
src/DotNetAiAssistant.Api
```

### Run

```bash
dotnet run --project src/DotNetAiAssistant.Api/DotNetAiAssistant.Api.csproj
```

Configure the API key using environment variables or .NET user secrets rather than committing credentials.

Example environment configuration:

```text
OpenAI__ApiKey=your_api_key
OpenAI__Model=gpt-4o-mini
```

## API

### Health

```http
GET /health
```

### Ask a question

```http
POST /api/ask
Content-Type: application/json
```

```json
{
  "question": "What is required before a production release?"
}
```

The response contains an answer and the knowledge sources used.

## Frontend

The React application is in the `frontend` directory.

```bash
cd frontend
npm install
npm run dev
```

## Tests

```bash
dotnet test tests/DotNetAiAssistant.Tests/DotNetAiAssistant.Tests.csproj
```

The unit tests cover retrieval behavior and help keep the knowledge layer independently testable.

## CI/CD

GitHub Actions runs the .NET test suite on:

- pushes to `main`;
- pull requests.

## Docker

```bash
docker build -t dotnet-ai-assistant .
docker run -p 8080:8080 -e OpenAI__ApiKey=your_api_key dotnet-ai-assistant
```

## Azure evolution

The design can evolve naturally toward:

- Azure OpenAI
- Azure AI Search
- Azure App Service or Container Apps
- Azure Key Vault
- managed identities
- Application Insights
- vector/semantic retrieval
- distributed tracing
- API authentication and authorization
- automated LLM evaluation

## Engineering decisions

**Service abstraction**  
The API depends on `IAiService` rather than a specific model implementation.

**Grounded context**  
The model receives only context selected from the knowledge layer.

**Safe configuration**  
Secrets are not stored in source control.

**Source visibility**  
API responses include source names so model answers are easier to inspect.

**Separation of concerns**  
Frontend, API, retrieval, and model integration are kept separate.

## Why I built this

I wanted a compact example showing how traditional enterprise .NET engineering can be combined with modern AI capabilities without turning the entire application into model-specific code.

This repository is an independent portfolio project and contains no proprietary employer code or confidential data.
