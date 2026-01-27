
using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = new([
        new GameDto(
        1,
        "Street Fighter II",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)),
    new GameDto(
        2,
        "Final Fantasy VII",
        "RPG",
        69.99M,
        new DateOnly(2024, 02, 29)),
    new GameDto(
        3,
        "Super Mario Bros.",
        "Platformer",
        19.99M,
        new DateOnly(1985, 11, 21))
    ]);

    //extension method to map the endpoints
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        //GET /games
        group.MapGet("/", () => games);

        //GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Add(game);
            //redirect to the route with the paylod of the new game 
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        //PUT /games/id
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            //find the index of game to update
            var index = games.FindIndex(game => game.Id == id);
            //if the game is not found
            if (index == -1)
            {
                return Results.NotFound();
            }
            //update the game
            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            return Results.NoContent();
        });

        //DELETE /games/id
        group.MapDelete("/{id}", (int id) =>
        {
            //find the index of game to delete
            var index = games.FindIndex(game => game.Id == id);
            //remove the game from the list
            games.RemoveAt(index);
            //or 
            //games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

    }

}
