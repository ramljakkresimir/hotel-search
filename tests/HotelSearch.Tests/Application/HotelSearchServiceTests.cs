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

    private sealed class FakePromptParser : IPromptParser
    {
        public SearchCriteria Parse(string prompt)
        {
            return new SearchCriteria("Zagreb", 20);
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