using HotelSearch.Domain.ValueObjects;

namespace HotelSearch.Application.Services;

public sealed class GeoDistanceService
{
    private const double EarthRadiusKm = 6371;

    public double CalculateDistanceInKm(GeoLocation first, GeoLocation second)
    {
        double latitudeDistance = ToRadians(second.Latitude - first.Latitude);
        double longitudeDistance = ToRadians(second.Longitude - first.Longitude);

        double firstLatitude = ToRadians(first.Latitude);
        double secondLatitude = ToRadians(second.Latitude);

        double a =
            Math.Sin(latitudeDistance / 2) * Math.Sin(latitudeDistance / 2) +
            Math.Cos(firstLatitude) * Math.Cos(secondLatitude) *
            Math.Sin(longitudeDistance / 2) * Math.Sin(longitudeDistance / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusKm * c;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}