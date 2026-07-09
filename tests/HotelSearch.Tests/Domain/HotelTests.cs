using HotelSearch.Domain.Entities;
using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Tests.Domain;

public sealed class HotelTests
{
    [Fact]
    public void Constructor_ShouldCreateHotel_WhenDataIsValid()
    {
        var location = new GeoLocation(45.815, 15.982);

        var hotel = new Hotel(Guid.NewGuid(), "Hotel Zagreb", 120, location);

        Assert.Equal("Hotel Zagreb", hotel.Name);
        Assert.Equal(120, hotel.Price);
        Assert.Equal(location, hotel.Location);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenPriceIsLessThanOrEqualToZero()
    {
        var location = new GeoLocation(45.815, 15.982);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Hotel(Guid.NewGuid(), "Invalid Hotel", 0, location)
        );
    }
}