using HotelSearch.Application.Abstractions;
using HotelSearch.Infrastructure.Repositories;
using HotelSearch.Application.Services;
using HotelSearch.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using HotelSearch.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateHotelRequestValidator>();

builder.Services.AddSingleton<IHotelRepository, InMemoryHotelRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<GeoDistanceService>();
builder.Services.AddSingleton<HotelSearchService>();

builder.Services.AddSingleton<GeoDistanceService>();
builder.Services.AddSingleton<HotelSearchService>();

builder.Services.AddSingleton<IPromptParser, SimplePromptParser>();

builder.Services.AddHttpClient<IGeocodingService, NominatimGeocodingService>(client =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("HotelSearchApi/1.0 (take-home-assignment)");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

