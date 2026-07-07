using HotelSearch.Api.Dtos;
using HotelSearch.Application.Abstractions;
using HotelSearch.Domain.Entities;
using HotelSearch.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using HotelSearch.Application.Services;

namespace HotelSearch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HotelsController : ControllerBase
{
    private readonly IHotelRepository _hotelRepository;
    private readonly HotelSearchService _hotelSearchService;

    public HotelsController(IHotelRepository hotelRepository, HotelSearchService hotelSearchService)
    {
        _hotelRepository = hotelRepository;
        _hotelSearchService = hotelSearchService;
    }

    [HttpPost]
    public async Task<ActionResult<HotelResponse>> Create(CreateHotelRequest request)
    {
        try
        {
            var hotel = new Hotel(
                Guid.NewGuid(),
                request.Name,
                request.Price,
                new GeoLocation(request.Latitude, request.Longitude)
            );

            await _hotelRepository.AddAsync(hotel);

            var response = ToResponse(hotel);

            return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<HotelResponse>>> GetAll()
    {
        var hotels = await _hotelRepository.GetAllAsync();

        var response = hotels
            .Select(ToResponse)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HotelResponse>> GetById(Guid id)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id);

        if (hotel is null)
            return NotFound();

        return Ok(ToResponse(hotel));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<HotelResponse>> Update(Guid id, UpdateHotelRequest request)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id);

        if (hotel is null)
            return NotFound();

        try
        {
            hotel.Update(
                request.Name,
                request.Price,
                new GeoLocation(request.Latitude, request.Longitude)
            );

            await _hotelRepository.UpdateAsync(hotel);

            return Ok(ToResponse(hotel));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool deleted = await _hotelRepository.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("search")]
    public async Task<ActionResult<PagedHotelSearchResult>> Search(SearchHotelsRequest request)
    {
        try
        {
            var result = await _hotelSearchService.SearchAsync(
                request.Prompt,
                request.Page,
                request.PageSize
            );

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private static HotelResponse ToResponse(Hotel hotel)
    {
        return new HotelResponse(
            hotel.Id,
            hotel.Name,
            hotel.Price,
            hotel.Location.Latitude,
            hotel.Location.Longitude
        );
    }
}