using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DotNetAiAssistant.Api.Services;

public sealed class OpenAiService(HttpClient httpClient, IConfiguration configuration) : IAiService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;

    public async Task<string> GenerateAnswerAsync(
        string question,
        IReadOnlyList<string> context,
        CancellationToken cancellationToken = default)
    {
        if (context.Count == 0)
        {
            return "I do not have enough information in the knowledge base to answer that.";
        }

        var apiKey = _configuration["OpenAI:ApiKey"];
        var model = _configuration["OpenAI:Model"] ?? "gpt-4o-mini";

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return $"Relevant context was found. Top source: {context[0]}";
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.openai.com/v1/chat/completions");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var prompt = $"""
You are a concise enterprise knowledge assistant.
Answer only from the supplied context. If the context is insufficient, say so.

Context:
{string.Join(Environment.NewLine + Environment.NewLine, context)}

Question:
{question}
""";

        var payload = JsonSerializer.Serialize(new
        {
            model,
            messages = new[]
            {
                new { role = "system", content = "Return a concise, factual answer grounded only in the supplied context." },
                new { role = "user", content = prompt }
            }
        });

        request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

        return document.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()
            ?.Trim()
            ?? "No answer was returned.";
    }
}
