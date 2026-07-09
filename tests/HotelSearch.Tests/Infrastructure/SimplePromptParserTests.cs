using HotelSearch.Infrastructure.Services;

namespace HotelSearch.Tests.Infrastructure;

public sealed class SimplePromptParserTests
{
    [Fact]
    public void Parse_ShouldExtractLocationAndBudget_FromEnglishPrompt()
    {
        var parser = new SimplePromptParser();

        var result = parser.Parse("Looking for a hotel in Zagreb under 150 EUR");

        Assert.Equal("Zagreb", result.LocationName);
        Assert.Equal(150, result.Budget);
    }

    [Fact]
    public void Parse_ShouldExtractLocationAndBudget_FromCroatianPrompt()
    {
        var parser = new SimplePromptParser();

        var result = parser.Parse("Hotel u Split do 200 EUR");

        Assert.Equal("Split", result.LocationName);
        Assert.Equal(200, result.Budget);
    }
}