using FluentValidation;
using HotelSearch.Api.Dtos;

namespace HotelSearch.Api.Validators;

public sealed class SearchHotelsRequestValidator : AbstractValidator<SearchHotelsRequest>
{
    public SearchHotelsRequestValidator()
    {
        RuleFor(x => x.Prompt)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}