# Architecture

## Request flow

1. The React UI sends a question to the ASP.NET Core API.
2. The API validates the request.
3. The knowledge service identifies relevant internal context.
4. The AI service sends only the retrieved context and question to the configured LLM.
5. The API returns the answer and source names to the client.

## Design choices

- the AI integration is behind an interface so the model provider can be replaced;
- retrieval is separated from request handling;
- model credentials live in configuration, not source control;
- the service still behaves safely when an API key is not configured;
- the API returns source names to make answers easier to inspect;
- the frontend is independent from the backend and communicates through REST.

## Production evolution

A production system could add Azure OpenAI, Azure AI Search, managed identities, Key Vault, Application Insights, distributed tracing, semantic/vector retrieval, rate limiting, caching, authentication, authorization, prompt versioning, and automated LLM evaluations.
