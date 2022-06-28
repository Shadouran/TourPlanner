using Microsoft.AspNetCore.Mvc;
using TourPlanner.Server.DAL;
using TourPlanner.Server.DAL.Records;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConfiguration>((_) =>
{
    return new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();
});
builder.Services.AddSingleton<INpgsqlDatabase, NpgsqlDatabase>();
builder.Services.AddSingleton<ITourRepository, TourRepositoryPostgreSQL>();
var app = builder.Build();

app.MapGet("/tours", async ([FromServices] ITourRepository tourRepository) =>
{
    var tours = await tourRepository.GetAllAsync();
    return Results.Ok(tours);
});

app.MapGet("/tours/{id}", async ([FromServices] ITourRepository tourRepository, Guid id) =>
{
    var tour = await tourRepository.GetByIdAsync(id);
    return tour is not null ? Results.Ok(tour) : Results.NotFound();
});

app.MapPost("/tours", async ([FromServices] ITourRepository tourRepository, Tour tour) =>
{
    await tourRepository.CreateAsync(tour);
    return Results.Created($"/tours/{tour.TourUserInformation.Id}", tour);
});

app.MapPut("/tours", async ([FromServices] ITourRepository tourRepository, Tour tour) =>
{
    await tourRepository.UpdateAsync(tour);
    return Results.Ok(tour);
});

app.MapDelete("/tours/{id}", async ([FromServices] ITourRepository tourRepository, Guid id) =>
{
    await tourRepository.DeleteAsync(id);
    return Results.Ok(id);
});

var host = $"{app.Services.GetRequiredService<IConfiguration>()["BaseUri"]}:{app.Services.GetRequiredService<IConfiguration>()["Port"]}";
app.Run(host);
