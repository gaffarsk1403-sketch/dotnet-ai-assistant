namespace DotNetAiAssistant.Api.Services;

public interface IAiService
{
    Task<string> GenerateAnswerAsync(
        string question,
        IReadOnlyList<string> context,
        CancellationToken cancellationToken = default);
}
