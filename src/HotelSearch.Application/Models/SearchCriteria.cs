namespace HotelSearch.Application.Models;

public sealed record SearchCriteria(
    string? LocationName,
    decimal? Budget
);