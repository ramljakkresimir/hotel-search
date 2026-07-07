using HotelSearch.Application.Abstractions;

namespace HotelSearch.Application.Services;

public sealed class HotelSearchService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IPromptParser _promptParser;
    private readonly IGeocodingService _geocodingService;
    private readonly GeoDistanceService _geoDistanceService;

    public HotelSearchService(
        IHotelRepository hotelRepository,
        IPromptParser promptParser,
        IGeocodingService geocodingService,
        GeoDistanceService geoDistanceService)
    {
        _hotelRepository = hotelRepository;
        _promptParser = promptParser;
        _geocodingService = geocodingService;
        _geoDistanceService = geoDistanceService;
    }

    public async Task<PagedHotelSearchResult> SearchAsync(
        string prompt,
        int page,
        int pageSize)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new ArgumentException("Search prompt is required.", nameof(prompt));

        if (page < 1)
            page = 1;

        if (pageSize < 1)
            pageSize = 10;

        if (pageSize > 100)
            pageSize = 100;

        var criteria = _promptParser.Parse(prompt);

        if (string.IsNullOrWhiteSpace(criteria.LocationName))
            throw new ArgumentException("Location could not be extracted from the search prompt.");

        var searchLocation = await _geocodingService.GeocodeAsync(criteria.LocationName);

        if (searchLocation is null)
            throw new ArgumentException("Location could not be resolved to coordinates.");

        var hotels = await _hotelRepository.GetAllAsync();

        var hotelResults = hotels
        .Select(hotel =>
        {
            double distanceKm = _geoDistanceService.CalculateDistanceInKm(
                searchLocation,
                hotel.Location);

            return new
            {
                Hotel = hotel,
                Distance = distanceKm
            };
        })
        .Where(x => criteria.Budget == null || x.Hotel.Price <= criteria.Budget.Value)
        .ToList();

    decimal maxPrice = hotelResults.Max(x => x.Hotel.Price);
    double maxDistance = hotelResults.Max(x => x.Distance);

        var rankedHotels = hotelResults
        .Select(x =>
        {
            double normalizedPrice =
                maxPrice == 0 ? 0 : (double)(x.Hotel.Price / maxPrice);

            double normalizedDistance =
                maxDistance == 0 ? 0 : x.Distance / maxDistance;

            double score = normalizedPrice + normalizedDistance;

            return new HotelSearchItem(
                x.Hotel.Id,
                x.Hotel.Name,
                x.Hotel.Price,
                Math.Round(x.Distance, 2),
                Math.Round(score, 3)
            );
        })
        .OrderBy(x => x.Score)
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
}

public sealed record HotelSearchItem(
    Guid Id,
    string Name,
    decimal Price,
    double DistanceKm,
    double Score
);

public sealed record PagedHotelSearchResult(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<HotelSearchItem> Items
);