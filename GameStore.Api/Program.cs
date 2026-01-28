
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var app = builder.Build();

//call the extension method
app.MapGamesEndpoints();

app.Run();
