using webapi.Dtos.Game;

namespace webapi.Routes;

public static class GameRoutes
{
    private const string RoutePrefix = "games";

    private static readonly List<GameDto> games = [
        new ("Adventure Quest", "Adventure", 29.99m, new DateOnly(2022, 5, 15)),
        new ("Space Invaders", "Arcade", 19.99m, new DateOnly(2021, 11, 20)),
        new ("Mystery Manor", "Puzzle", 14.99m, new DateOnly(2023, 2, 10))
    ];
    
    public static void MapGameRoutes(this WebApplication app)
    {
        var group = app.MapGroup(RoutePrefix);

        group.MapGet("/", () => Results.Ok(games))
            .WithName("GetAllGames")
            .WithTags("Games");

        group.MapGet("/{name}", (string name) =>
        {
            var game = games.FirstOrDefault(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return game is not null ? Results.Ok(game) : Results.NotFound();
        })
        .WithName("GetGameByName")
        .WithTags("Games");

        group.MapPost("/", (GameDto newGame) =>
        {
            games.Add(newGame);
            return Results.Created($"/{RoutePrefix}/{newGame.Name}", newGame);
        })
        .WithName("CreateGame")
        .WithTags("Games");

        group.MapPut("/{name}", (string name, GameDto updatedGame) =>
        {
            var index = games.FindIndex(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (index == -1)
            {
                return Results.NotFound();
            }
            games[index] = updatedGame;
            return Results.NoContent();
        })
        .WithName("UpdateGame")
        .WithTags("Games");

        group.MapDelete("/{name}", (string name) =>
        {
            var game = games.FirstOrDefault(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (game is null)
            {
                return Results.NotFound();
            }
            games.Remove(game);
            return Results.NoContent();
        })
        .WithName("DeleteGame")
        .WithTags("Games");
    }
}