using HotelSearch.Application.Models;

namespace HotelSearch.Application.Abstractions;

public interface IPromptParser
{
    SearchCriteria Parse(string prompt);
}