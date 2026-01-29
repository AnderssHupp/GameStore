using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    //extension method to map the endpoints
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        //GET /games
        group.MapGet("/", async (GameStoreContext dbContext)
            => await dbContext.Games

                .Include(game => game.Genre) //load the genre
                .Select
                (
                    game => new GameSummaryDto(
                        game.Id,
                        game.Name,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate
                )
            )
            .AsNoTracking()
            .ToListAsync()
        );

        //GET /games/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            //find the game by id
            var game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );
        })
        .WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game); //call the dbContext
            //need to be async method to save
            await dbContext.SaveChangesAsync();
            //create the game dto details 
            //CONTRACT to send details to client
            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            //redirect to the route with the paylod of the new game 
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        //PUT /games/id
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            //load the game to update
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            //persist the changes
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //DELETE /games/id
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            //bulk deletion
            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();

            //var existingGame = await dbContext.Games.FindAsync(id);
            /*if (existingGame is null)
            {
                return Results.NotFound();
            }
            //remove the game from the listS
            //dbContext.Games.Remove(existingGame);
            await dbContext.SaveChangesAsync();*/

            return Results.NoContent();
        });

    }

}
