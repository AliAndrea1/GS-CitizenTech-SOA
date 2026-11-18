
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Oasis.Soa.Domain.Models;
using Xunit;

namespace Oasis.Soa.Tests;

public class WeatherReadingTests
{
    private static List<ValidationResult> Validate(
        WeatherReading reading)
    {
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(
            reading,
            new ValidationContext(reading),
            results,
            validateAllProperties: true
        );

        return results;
    }

    [Fact]
    public void WeatherReading_DeveGerarIdAutomaticamente()
    {
        var reading = new WeatherReading();

        Assert.NotEqual(Guid.Empty, reading.Id);
    }

    [Fact]
    public void WeatherReading_DeveTerFontePadrao()
    {
        var reading = new WeatherReading();

        Assert.Equal("Open-Meteo", reading.Source);
    }

    [Fact]
    public void WeatherReading_DeveRegistrarDataCriacao()
    {
        var antes = DateTime.UtcNow;

        var reading = new WeatherReading();

        var depois = DateTime.UtcNow;

        Assert.InRange(reading.CreatedAt, antes, depois);
    }

    [Theory]
    [InlineData(-23.5432, -46.6427)]
    [InlineData(0, 0)]
    [InlineData(90, 180)]
    [InlineData(-90, -180)]
    public void WeatherReading_DeveAceitarCoordenadasValidas(
        double latitude,
        double longitude)
    {
        var reading = new WeatherReading
        {
            Latitude = latitude,
            Longitude = longitude
        };

        Assert.Empty(Validate(reading));
    }

    [Theory]
    [InlineData(-91, 0)]
    [InlineData(91, 0)]
    [InlineData(0, -181)]
    [InlineData(0, 181)]
    public void WeatherReading_DeveRejeitarCoordenadasInvalidas(
        double latitude,
        double longitude)
    {
        var reading = new WeatherReading
        {
            Latitude = latitude,
            Longitude = longitude
        };

        Assert.NotEmpty(Validate(reading));
    }

    [Theory]
    [InlineData(-90)]
    [InlineData(0)]
    [InlineData(24.6)]
    [InlineData(60)]
    public void WeatherReading_DeveAceitarTemperaturasValidas(
        double temperatura)
    {
        var reading = new WeatherReading
        {
            TemperatureC = temperatura
        };

        Assert.Empty(Validate(reading));
    }

    [Theory]
    [InlineData(-91)]
    [InlineData(61)]
    public void WeatherReading_DeveRejeitarTemperaturasInvalidas(
        double temperatura)
    {
        var reading = new WeatherReading
        {
            TemperatureC = temperatura
        };

        Assert.NotEmpty(Validate(reading));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(75)]
    [InlineData(100)]
    public void WeatherReading_DeveAceitarUmidadeValida(
        int umidade)
    {
        var reading = new WeatherReading
        {
            RelativeHumidity = umidade
        };

        Assert.Empty(Validate(reading));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void WeatherReading_DeveRejeitarUmidadeInvalida(
        int umidade)
    {
        var reading = new WeatherReading
        {
            RelativeHumidity = umidade
        };

        Assert.NotEmpty(Validate(reading));
    }

    [Fact]
    public void WeatherReading_DeveRejeitarPrecipitacaoNegativa()
    {
        var reading = new WeatherReading
        {
            PrecipitationMm = -1
        };

        Assert.NotEmpty(Validate(reading));
    }

    [Fact]
    public void WeatherReading_DeveAceitarPrecipitacaoZero()
    {
        var reading = new WeatherReading
        {
            PrecipitationMm = 0
        };

        Assert.Empty(Validate(reading));
    }
}
