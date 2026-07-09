using HotelSearch.Application.Abstractions;
using HotelSearch.Application.Models;
using HotelSearch.Application.Services;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Tests.Application;

public sealed class HotelSearchServiceTests
{
    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyResult_WhenNoHotelsMatchBudget()
    {
        var repository = new FakeHotelRepository();
        var promptParser = new FakePromptParser();
        var geocodingService = new FakeGeocodingService();

        await repository.AddAsync(new Hotel(
            Guid.NewGuid(),
            "Hotel Zagreb",
            25,
            new GeoLocation(45.815, 15.982)
        ));

        var service = new HotelSearchService(
            repository,
            promptParser,
            geocodingService,
            new GeoDistanceService()
        );

        var result = await service.SearchAsync(
            "Looking for a hotel in Zagreb under 20 EUR",
            1,
            10
        );

        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnRankedAndPaginatedResults()
    {
        var repository = new FakeHotelRepository();
        var promptParser = new FakePromptParser("Zagreb", 200);
        var geocodingService = new FakeGeocodingService();

        await repository.AddAsync(new Hotel(
            Guid.NewGuid(),
            "Far Expensive Hotel",
            180,
            new GeoLocation(43.508, 16.440)
        ));

        await repository.AddAsync(new Hotel(
            Guid.NewGuid(),
            "Close Cheap Hotel",
            80,
            new GeoLocation(45.815, 15.982)
        ));

        await repository.AddAsync(new Hotel(
            Guid.NewGuid(),
            "Close Expensive Hotel",
            150,
            new GeoLocation(45.816, 15.983)
        ));

        var service = new HotelSearchService(
            repository,
            promptParser,
            geocodingService,
            new GeoDistanceService()
        );

        var result = await service.SearchAsync(
            "Looking for a hotel in Zagreb under 200 EUR",
            1,
            2
        );

        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("Close Cheap Hotel", result.Items.First().Name);
    }

    private sealed class FakePromptParser : IPromptParser
    {
        private readonly string? _locationName;
        private readonly decimal? _budget;

        public FakePromptParser(string? locationName = "Zagreb", decimal? budget = 20)
        {
            _locationName = locationName;
            _budget = budget;
        }

        public SearchCriteria Parse(string prompt)
        {
            return new SearchCriteria(_locationName, _budget);
        }
    }

    private sealed class FakeGeocodingService : IGeocodingService
    {
        public Task<GeoLocation?> GeocodeAsync(string locationName)
        {
            return Task.FromResult<GeoLocation?>(new GeoLocation(45.815, 15.982));
        }
    }

    private sealed class FakeHotelRepository : IHotelRepository
    {
        private readonly List<Hotel> _hotels = [];

        public Task<Hotel> AddAsync(Hotel hotel)
        {
            _hotels.Add(hotel);
            return Task.FromResult(hotel);
        }

        public Task<Hotel?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_hotels.FirstOrDefault(x => x.Id == id));
        }

        public Task<IReadOnlyCollection<Hotel>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyCollection<Hotel>>(_hotels.AsReadOnly());
        }

        public Task<bool> UpdateAsync(Hotel hotel)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return Task.FromResult(true);
        }
    }
}