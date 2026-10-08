namespace DotNetAiAssistant.Api.Models;

public sealed record AskResponse(string Answer, IReadOnlyList<string> Sources);
