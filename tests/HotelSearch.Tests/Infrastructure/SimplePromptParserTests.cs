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

    [Fact]
    public void Parse_ShouldNotTreatDoInsideLocationNameAsBudgetKeyword()
    {
        var parser = new SimplePromptParser();

        var result = parser.Parse("Looking for a hotel in Toledo 300 reviews");

        Assert.Equal("Toledo 300 reviews", result.LocationName);
        Assert.Null(result.Budget);
    }

    [Fact]
    public void Parse_ShouldExtractLocationAndBudget_WhenLocationContainsDo()
    {
        var parser = new SimplePromptParser();

        var result = parser.Parse("Looking for a hotel in Toledo under 150 EUR");

        Assert.Equal("Toledo", result.LocationName);
        Assert.Equal(150, result.Budget);
    }
}