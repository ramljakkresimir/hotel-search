using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Application.Abstractions;

public interface IGeocodingService
{
    Task<GeoLocation?> GeocodeAsync(string locationName);
}