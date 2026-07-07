using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Domain.Entities;

public sealed class Hotel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public GeoLocation Location { get; private set; }

    public Hotel(Guid id, string name, decimal price, GeoLocation location)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Hotel id cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Hotel name is required.", nameof(name));

        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Hotel price must be greater than zero.");

        Id = id;
        Name = name.Trim();
        Price = price;
        Location = location ?? throw new ArgumentNullException(nameof(location));
    }

    public void Update(string name, decimal price, GeoLocation location)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Hotel name is required.", nameof(name));

        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Hotel price must be greater than zero.");

        Name = name.Trim();
        Price = price;
        Location = location ?? throw new ArgumentNullException(nameof(location));
    }
}