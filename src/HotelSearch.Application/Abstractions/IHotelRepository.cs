using HotelSearch.Domain.Entities;

namespace HotelSearch.Application.Abstractions;

public interface IHotelRepository
{
    Task<Hotel> AddAsync(Hotel hotel);
    Task<Hotel?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Hotel>> GetAllAsync();
    Task<bool> UpdateAsync(Hotel hotel);
    Task<bool> DeleteAsync(Guid id);
}