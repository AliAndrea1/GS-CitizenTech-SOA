
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oasis.Soa.Api.Data;
using Oasis.Soa.Api.Services;
using System.Text.Json;

namespace Oasis.Soa.Api.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    private readonly OpenMeteoService _weatherService;
    private readonly OasisWeatherDbContext _context;

    public WeatherController(
        OpenMeteoService weatherService,
        OasisWeatherDbContext context)
    {
        _weatherService = weatherService;
        _context = context;
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent(
        [FromQuery] double? latitude,
        [FromQuery] double? longitude,
        CancellationToken cancellationToken)
    {
        if (!latitude.HasValue ||
            !longitude.HasValue ||
            !double.IsFinite(latitude.Value) ||
            !double.IsFinite(longitude.Value) ||
            latitude.Value < -90 ||
            latitude.Value > 90 ||
            longitude.Value < -180 ||
            longitude.Value > 180)
        {
            return BadRequest(new
            {
                message = "Informe latitude e longitude válidas."
            });
        }

        try
        {
            var reading = await _weatherService.GetCurrentWeatherAsync(
                latitude.Value,
                longitude.Value,
                cancellationToken
            );

            _context.WeatherReadings.Add(reading);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(reading);
        }
        catch (HttpRequestException)
        {
            return StatusCode(502, new
            {
                message = "Não foi possível consultar o serviço meteorológico."
            });
        }
        catch (JsonException)
        {
            return StatusCode(502, new
            {
                message = "O serviço meteorológico retornou dados inválidos."
            });
        }
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        if (limit < 1 || limit > 100)
        {
            return BadRequest(new
            {
                message = "O limite deve estar entre 1 e 100."
            });
        }

        var readings = await _context.WeatherReadings
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return Ok(readings);
    }
}
