using TourPlanner.Server.Modules;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConfiguration>((_) =>
{
    return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();
});
builder.Services.RegisterModules();

var app = builder.Build();
app.MapEndpoints();

var config = app.Services.GetRequiredService<IConfiguration>();
var host = $"{config["BaseAddress"]}:{config["Port"]}";
app.Run(host);
