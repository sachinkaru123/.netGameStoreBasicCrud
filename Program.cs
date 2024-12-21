using WebApplication1.Data;
using WebApplication1.Endpoints;


        var builder = WebApplication.CreateBuilder(args);

        var connStr = builder.Configuration.GetConnectionString("GameStore");
        builder.Services.AddSqlite<GameStoreContext> (connStr);

        var app = builder.Build();

        app.MapGamesEndPoints();

        await app.MigrationDbAsync();
        
        app.Run();
    