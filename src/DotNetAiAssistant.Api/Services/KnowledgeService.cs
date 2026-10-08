namespace DotNetAiAssistant.Api.Services;

public sealed class KnowledgeService
{
    private static readonly IReadOnlyDictionary<string, string> Documents =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Release Process"] =
                "Production releases require peer review, automated tests, staging validation, and a documented rollback plan.",
            ["Incident Response"] =
                "High-priority incidents are triaged by the on-call engineer and documented with impact, timeline, root cause, and follow-up actions.",
            ["Engineering Standards"] =
                "Services should include structured logging, input validation, automated tests, clear ownership, and operational documentation."
        };

    public IReadOnlyList<KeyValuePair<string, string>> Search(string question, int limit = 3)
    {
        var terms = question
            .ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(term => term.Length > 2)
            .ToHashSet();

        return Documents
            .Select(item => new
            {
                Item = item,
                Score = terms.Count(term =>
                    item.Key.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    item.Value.Contains(term, StringComparison.OrdinalIgnoreCase))
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(limit)
            .Select(x => x.Item)
            .ToList();
    }
}
