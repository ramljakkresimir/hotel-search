namespace HotelSearch.Api.Dtos;

public sealed record SearchHotelsRequest(
    string Prompt,
    int Page = 1,
    int PageSize = 10
);