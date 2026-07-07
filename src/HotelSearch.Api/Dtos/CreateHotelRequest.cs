namespace HotelSearch.Api.Dtos;

public sealed record CreateHotelRequest(
    string Name,
    decimal Price,
    double Latitude,
    double Longitude
);