
using System.Globalization;
using System.Text.Json;
using Oasis.Soa.Domain.Models;

namespace Oasis.Soa.Api.Services;

public class OpenMeteoService
{
    private readonly HttpClient _httpClient;

    public OpenMeteoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherReading> GetCurrentWeatherAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        string lat = latitude.ToString(CultureInfo.InvariantCulture);
        string lon = longitude.ToString(CultureInfo.InvariantCulture);

        string url =
            $"https://api.open-meteo.com/v1/forecast" +
            $"?latitude={lat}" +
            $"&longitude={lon}" +
            $"&current=temperature_2m,relative_humidity_2m,precipitation,apparent_temperature" +
            $"&timezone=America%2FSao_Paulo";

        using var response = await _httpClient.GetAsync(
            url,
            cancellationToken
        );

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(
            cancellationToken
        );

        using var json = await JsonDocument.ParseAsync(
            stream,
            cancellationToken: cancellationToken
        );

        var current = json.RootElement.GetProperty("current");

        double temperature = current
            .GetProperty("temperature_2m")
            .GetDouble();

        double apparentTemperature = current
            .GetProperty("apparent_temperature")
            .GetDouble();

        int humidity = current
            .GetProperty("relative_humidity_2m")
            .GetInt32();

        double precipitation = current
            .GetProperty("precipitation")
            .GetDouble();

        string observedTime = current
            .GetProperty("time")
            .GetString()!;

        var localTime = DateTime.Parse(
            observedTime,
            CultureInfo.InvariantCulture
        );

        var observedAt = DateTime.SpecifyKind(
            localTime,
            DateTimeKind.Unspecified
        );

        return new WeatherReading
        {
            Latitude = latitude,
            Longitude = longitude,
            TemperatureC = temperature,
            ApparentTemperatureC = apparentTemperature,
            RelativeHumidity = humidity,
            PrecipitationMm = precipitation,
            Source = "Open-Meteo",
            ObservedAt = observedAt
        };
    }
}
