using System;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Entities;
namespace WebApplication1.Endpoints;


public static class GameEndPoint
{
    private static readonly List<GameDto> games = [
         new (
        Id: 1,
        Name: "Mystic Quest",
        Genre: "Adventure",
        Price: 19.99m,
        ReleaseDate: new DateOnly(2023, 10, 15)
    ),
    new (
        Id: 2,
        Name: "Battle Zone X",
        Genre: "Action",
        Price: 49.99m,
        ReleaseDate: new DateOnly(2024, 1, 20)
    ),
    new (
        Id: 3,
        Name: "Puzzle Mania",
        Genre: "Puzzle",
        Price: 9.99m,
        ReleaseDate: new DateOnly(2022, 7, 5)
    )];

    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                        .WithParameterValidation(); //validation added , we must set dtos

        // get all games
        group.MapGet("/", () => games);

        // get specific game
        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);

        }).WithName("GetGame");

        //create new game
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {

            Game game = new(){
                Name = newGame.Name,
                Genre= dbContext.Genres.Find(newGame.GenreId),
                GenreId= newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            Console.WriteLine(game);

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            // GameDto game = new(
            //     games.Count + 1,
            //     newGame.Name,
            //     newGame.Genre,
            //     newGame.Price,
            //     newGame.ReleaseDate
            // );

            // games.Add(game);

            GameDto returnGameData = new(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, returnGameData);
        });

        //update game
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {

                return Results.NotFound();
            }
            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            return Results.NoContent();

        });

        //delete game
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;

    }
}
