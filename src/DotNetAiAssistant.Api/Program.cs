using DotNetAiAssistant.Api.Models;
using DotNetAiAssistant.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<KnowledgeService>();
builder.Services.AddHttpClient<IAiService, OpenAiService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/api/ask", async (
    AskRequest request,
    KnowledgeService knowledgeService,
    IAiService aiService,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.Question) || request.Question.Trim().Length < 3)
    {
        return Results.BadRequest(new { error = "Question must contain at least 3 characters." });
    }

    var matches = knowledgeService.Search(request.Question);
    var context = matches
        .Select(item => $"[{item.Key}] {item.Value}")
        .ToList();

    var answer = await aiService.GenerateAnswerAsync(
        request.Question.Trim(),
        context,
        cancellationToken);

    return Results.Ok(new AskResponse(
        answer,
        matches.Select(item => item.Key).ToList()));
});

app.Run();

public partial class Program;
