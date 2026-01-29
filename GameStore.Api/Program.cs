using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

//call the extension method to configure the db
builder.AddGameStoreDb();

var app = builder.Build();

//call the extension method to map the endpoints
app.MapGamesEndpoints();
//call the extension method to map the endpoints
app.MapGenreEndpoints();

app.MigrateDb();

app.Run();
