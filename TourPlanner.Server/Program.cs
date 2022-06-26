using Microsoft.AspNetCore.Mvc;
using TourPlanner.Server.DAL;
using TourPlanner.Server.DAL.Records;

var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
var configurationRoot = configurationBuilder.Build();
var databaseConnection = new DatabaseConnection(configurationRoot["ConnectionString"]);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITourRepository>(provider => new TourRepositoryPostgreSQL(databaseConnection));
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
    return Results.Created($"/tours/{tour.Id}", tour);
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

app.Run("http://localhost:3000");
