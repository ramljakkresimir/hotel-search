using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using HotelSearch.Application.Abstractions;
using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Infrastructure.Services;

public sealed class NominatimGeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;

    public NominatimGeocodingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GeoLocation?> GeocodeAsync(string locationName)
    {
        if (string.IsNullOrWhiteSpace(locationName))
            return null;

        string url =
            $"search?q={Uri.EscapeDataString(locationName)}&format=json&limit=1";

        var results = await _httpClient.GetFromJsonAsync<List<NominatimSearchResult>>(url);

        var firstResult = results?.FirstOrDefault();

        if (firstResult is null)
            return null;

        bool latitudeParsed = double.TryParse(
            firstResult.Latitude,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double latitude
        );

        bool longitudeParsed = double.TryParse(
            firstResult.Longitude,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double longitude
        );

        if (!latitudeParsed || !longitudeParsed)
            return null;

        return new GeoLocation(latitude, longitude);
    }

    private sealed record NominatimSearchResult(
        [property: JsonPropertyName("lat")] string Latitude,
        [property: JsonPropertyName("lon")] string Longitude
    );
}