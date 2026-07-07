namespace HotelSearch.Api.Dtos;

public sealed record UpdateHotelRequest(
    string Name,
    decimal Price,
    double Latitude,
    double Longitude
);