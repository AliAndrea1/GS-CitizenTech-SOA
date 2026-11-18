
using Microsoft.EntityFrameworkCore;
using Oasis.Soa.Domain.Models;

namespace Oasis.Soa.Api.Data;

public class OasisWeatherDbContext : DbContext
{
    public OasisWeatherDbContext(
        DbContextOptions<OasisWeatherDbContext> options
    ) : base(options)
    {
    }

    public DbSet<WeatherReading> WeatherReadings =>
        Set<WeatherReading>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherReading>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Source)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => x.ObservedAt);
        });
    }
}
