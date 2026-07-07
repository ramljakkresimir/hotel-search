using HotelSearch.Application.Abstractions;
using HotelSearch.Domain.Entities;

namespace HotelSearch.Infrastructure.Repositories;

public sealed class InMemoryHotelRepository : IHotelRepository
{
    private readonly List<Hotel> _hotels = [];

    public Task<Hotel> AddAsync(Hotel hotel)
    {
        _hotels.Add(hotel);
        return Task.FromResult(hotel);
    }

    public Task<Hotel?> GetByIdAsync(Guid id)
    {
        Hotel? hotel = _hotels.FirstOrDefault(hotel => hotel.Id == id);
        return Task.FromResult(hotel);
    }

    public Task<IReadOnlyCollection<Hotel>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyCollection<Hotel>>(_hotels.AsReadOnly());
    }

    public Task<bool> UpdateAsync(Hotel hotel)
    {
        int index = _hotels.FindIndex(existingHotel => existingHotel.Id == hotel.Id);

        if (index == -1)
            return Task.FromResult(false);

        _hotels[index] = hotel;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        Hotel? hotel = _hotels.FirstOrDefault(hotel => hotel.Id == id);

        if (hotel is null)
            return Task.FromResult(false);

        _hotels.Remove(hotel);
        return Task.FromResult(true);
    }
}