using DotNetAiAssistant.Api.Services;

namespace DotNetAiAssistant.Tests;

public sealed class KnowledgeServiceTests
{
    [Fact]
    public void Search_ReturnsReleaseProcess_ForReleaseQuestion()
    {
        var service = new KnowledgeService();

        var result = service.Search("What is required before a production release?");

        Assert.NotEmpty(result);
        Assert.Equal("Release Process", result[0].Key);
    }

    [Fact]
    public void Search_ReturnsEmpty_ForUnrelatedQuestion()
    {
        var service = new KnowledgeService();

        var result = service.Search("What is on the cafeteria menu?");

        Assert.Empty(result);
    }
}
