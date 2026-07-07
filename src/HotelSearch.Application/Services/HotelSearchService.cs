using HotelSearch.Application.Abstractions;
using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Application.Services;

public sealed class HotelSearchService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly GeoDistanceService _geoDistanceService;

    public HotelSearchService(
        IHotelRepository hotelRepository,
        GeoDistanceService geoDistanceService)
    {
        _hotelRepository = hotelRepository;
        _geoDistanceService = geoDistanceService;
    }

    public async Task<PagedHotelSearchResult> SearchAsync(
        string? prompt,
        double currentLatitude,
        double currentLongitude,
        int page,
        int pageSize)
    {
        if (page < 1)
            page = 1;

        if (pageSize < 1)
            pageSize = 10;

        if (pageSize > 100)
            pageSize = 100;

        decimal? budget = ExtractBudget(prompt);

        var currentLocation = new GeoLocation(currentLatitude, currentLongitude);

        var hotels = await _hotelRepository.GetAllAsync();

        var rankedHotels = hotels
            .Select(hotel =>
            {
                double distanceKm = _geoDistanceService.CalculateDistanceInKm(
                    currentLocation,
                    hotel.Location);

                return new HotelSearchItem(
                    hotel.Id,
                    hotel.Name,
                    hotel.Price,
                    Math.Round(distanceKm, 2)
                );
            })
            .Where(hotel => budget is null || hotel.Price <= budget.Value)
            .OrderBy(hotel => hotel.Price)
            .ThenBy(hotel => hotel.DistanceKm)
            .ToList();

        int totalCount = rankedHotels.Count;

        var items = rankedHotels
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedHotelSearchResult(
            page,
            pageSize,
            totalCount,
            items
        );
    }

    private static decimal? ExtractBudget(string? prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            return null;

        var words = prompt.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string word in words)
        {
            string normalizedWord = word
                .Replace("€", "")
                .Replace("EUR", "", StringComparison.OrdinalIgnoreCase)
                .Replace(",", "")
                .Replace(".", "");

            if (decimal.TryParse(normalizedWord, out decimal value))
                return value;
        }

        return null;
    }
}

public sealed record HotelSearchItem(
    Guid Id,
    string Name,
    decimal Price,
    double DistanceKm
);

public sealed record PagedHotelSearchResult(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<HotelSearchItem> Items
);