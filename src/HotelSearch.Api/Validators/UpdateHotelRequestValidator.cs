using FluentValidation;
using HotelSearch.Api.Dtos;

namespace HotelSearch.Api.Validators;

public sealed class UpdateHotelRequestValidator : AbstractValidator<UpdateHotelRequest>
{
    public UpdateHotelRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180);
    }
}