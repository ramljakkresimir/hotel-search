namespace HotelSearch.Api.Dtos;

public sealed record SearchHotelsRequest(
    string? Prompt,
    double CurrentLatitude,
    double CurrentLongitude,
    int Page = 1,
    int PageSize = 10
);