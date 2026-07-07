using System.Text.RegularExpressions;
using HotelSearch.Application.Abstractions;
using HotelSearch.Application.Models;

namespace HotelSearch.Infrastructure.Services;

public sealed class SimplePromptParser : IPromptParser
{
    private static readonly Regex BudgetRegex = new(
        @"(?i)(under|max|maximum|budget|do|ispod)\s*(\d+)",
        RegexOptions.Compiled
    );

    public SearchCriteria Parse(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            return new SearchCriteria(null, null);

        decimal? budget = ExtractBudget(prompt);
        string? location = ExtractLocation(prompt);

        return new SearchCriteria(location, budget);
    }

    private static decimal? ExtractBudget(string prompt)
    {
        var match = BudgetRegex.Match(prompt);

        if (!match.Success)
            return null;

        return decimal.TryParse(match.Groups[2].Value, out decimal budget)
            ? budget
            : null;
    }

    private static string? ExtractLocation(string prompt)
    {
        string[] keywords = [" in ", " near ", " around ", " u ", " blizu "];

        foreach (string keyword in keywords)
        {
            int index = prompt.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);

            if (index == -1)
                continue;

            string locationPart = prompt[(index + keyword.Length)..];

            locationPart = Regex.Replace(
                locationPart,
                @"(?i)\s*(under|max|maximum|budget|do|ispod)\s*\d+.*",
                ""
            );

            locationPart = locationPart
                .Replace("€", "")
                .Replace("EUR", "", StringComparison.OrdinalIgnoreCase)
                .Trim();

            return string.IsNullOrWhiteSpace(locationPart) ? null : locationPart;
        }

        return null;
    }
}