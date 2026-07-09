using System.Collections.Concurrent;
using HotelSearch.Application.Abstractions;
using HotelSearch.Domain.Entities;

namespace HotelSearch.Infrastructure.Repositories;

public sealed class InMemoryHotelRepository : IHotelRepository
{
    private readonly ConcurrentDictionary<Guid, Hotel> _hotels = new();

    public Task<Hotel> AddAsync(Hotel hotel)
    {
        _hotels[hotel.Id] = hotel;
        return Task.FromResult(hotel);
    }

    public Task<Hotel?> GetByIdAsync(Guid id)
    {
        _hotels.TryGetValue(id, out Hotel? hotel);
        return Task.FromResult(hotel);
    }

    public Task<IReadOnlyCollection<Hotel>> GetAllAsync()
    {
        IReadOnlyCollection<Hotel> hotels = _hotels.Values.ToList();
        return Task.FromResult(hotels);
    }

    public Task<bool> UpdateAsync(Hotel hotel)
    {
        if (!_hotels.ContainsKey(hotel.Id))
            return Task.FromResult(false);

        _hotels[hotel.Id] = hotel;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(_hotels.TryRemove(id, out _));
    }
}