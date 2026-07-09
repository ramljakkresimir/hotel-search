using HotelSearch.Application.Services;
using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Tests.Application;

public sealed class GeoDistanceServiceTests
{
    [Fact]
    public void CalculateDistanceInKm_ShouldReturnZero_WhenLocationsAreSame()
    {
        var service = new GeoDistanceService();

        var location = new GeoLocation(45.815, 15.982);

        double distance = service.CalculateDistanceInKm(location, location);

        Assert.Equal(0, distance, precision: 2);
    }

    [Fact]
    public void CalculateDistanceInKm_ShouldReturnPositiveDistance_WhenLocationsAreDifferent()
    {
        var service = new GeoDistanceService();

        var zagreb = new GeoLocation(45.815, 15.982);
        var split = new GeoLocation(43.508, 16.440);

        double distance = service.CalculateDistanceInKm(zagreb, split);

        Assert.True(distance > 0);
    }
}