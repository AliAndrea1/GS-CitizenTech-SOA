
using System;
using System.ComponentModel.DataAnnotations;

namespace Oasis.Soa.Domain.Models;

public class WeatherReading
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    [Range(-90, 60)]
    public double TemperatureC { get; set; }

    // Sensação térmica estimada pela Open-Meteo
    [Range(-100, 100)]
    public double? ApparentTemperatureC { get; set; }

    [Range(0, 100)]
    public int RelativeHumidity { get; set; }

    [Range(0, double.MaxValue)]
    public double PrecipitationMm { get; set; }

    [Required]
    [StringLength(100)]
    public string Source { get; set; } = "Open-Meteo";

    public DateTime ObservedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
