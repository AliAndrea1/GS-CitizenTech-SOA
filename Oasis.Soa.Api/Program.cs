using Microsoft.EntityFrameworkCore;
using Oasis.Soa.Api.Data;
using Oasis.Soa.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<OpenMeteoService>();


var databasePath = Path.Combine(
    builder.Environment.ContentRootPath,
    "oasis-weather.db"
);

builder.Services.AddDbContext<OasisWeatherDbContext>(options =>
    options.UseSqlite($"Data Source={databasePath}"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<OasisWeatherDbContext>();

    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
