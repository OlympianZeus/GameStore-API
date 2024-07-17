using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndPointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                        .WithParameterValidation();
        //GET games endpoint
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Games
                    .Include(game => game.Genre)//this includes the Genre 
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking()
                    .ToListAsync());

        //GET games/1 endpoint
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
            .WithName(GetGameEndPointName);

        //POST /games endpoint
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            //game.Genre = dbContext.Genres.Find(newGame.GenreId);

            dbContext.Games.Add(game);//Games is the dbset
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(
                GetGameEndPointName,
                new { id = game.Id },
                game.ToGameDetailsDto());//the returning message
        });//this is the function that is used to validate entries from the nuget package

        //PUT /games endpoint
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }
            dbContext.Entry(existingGame)
                    .CurrentValues
                    .SetValues(updatedGame.ToEntity(id)); //the Entry is the method that is used to locate the current entity inside this dbcontext
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //DELETE

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDeleteAsync();
            return Results.NoContent();
        });

        return group;
    }
}
