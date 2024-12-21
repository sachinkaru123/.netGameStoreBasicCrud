using System;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Entities;
using WebApplication1.Mappings;
namespace WebApplication1.Endpoints;


public static class GameEndPoint
{
    // private static readonly List<GameSummeryDto> games = [
    //      new (
    //     Id: 1,
    //     Name: "Mystic Quest",
    //     Genre: "Adventure",
    //     Price: 19.99m,
    //     ReleaseDate: new DateOnly(2023, 10, 15)
    // ),
    // new (
    //     Id: 2,
    //     Name: "Battle Zone X",
    //     Genre: "Action",
    //     Price: 49.99m,
    //     ReleaseDate: new DateOnly(2024, 1, 20)
    // ),
    // new (
    //     Id: 3,
    //     Name: "Puzzle Mania",
    //     Genre: "Puzzle",
    //     Price: 9.99m,
    //     ReleaseDate: new DateOnly(2022, 7, 5)
    // )];

    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                        .WithParameterValidation(); //validation added , we must set dtos

        // get all games
        group.MapGet("/", async (GameStoreContext dbcontext) =>
        await dbcontext.Games
                    .Include(game=>game.Genre)
                    .Select(game => game.ToGameSummeryDto())
                    .ToListAsync()
                    );

        // get specific game
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game =await dbContext.Games.FindAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

        }).WithName("GetGame");

        //create new game
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {

            Game game = newGame.ToEntity();
           
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.ToGameDetailsDto());
        });

        //update game
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existinGame=await dbContext.Games.FindAsync(id);

            if (existinGame is null)
            {

                return Results.NotFound();
            }

            dbContext.Entry(existinGame)
            .CurrentValues.SetValues(updatedGame.ToEntity(id));

           await dbContext.SaveChangesAsync();

            return Results.NoContent();

        });

        //delete game
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
          await dbContext.Games
                    .Where(game=>game.Id == id)
                    .ExecuteDeleteAsync();


            return Results.NoContent();
        });

        return group;

    }
}
