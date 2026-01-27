
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//call the extension method
app.MapGamesEndpoints();

app.Run();
