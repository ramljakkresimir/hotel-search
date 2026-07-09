using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Tests.Domain;

public sealed class GeoLocationTests
{
    [Fact]
    public void Constructor_ShouldCreateGeoLocation_WhenCoordinatesAreValid()
    {
        var location = new GeoLocation(45.815, 15.982);

        Assert.Equal(45.815, location.Latitude);
        Assert.Equal(15.982, location.Longitude);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenLatitudeIsInvalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GeoLocation(200, 15.982)
        );
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenLongitudeIsInvalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GeoLocation(45.815, 500)
        );
    }
}