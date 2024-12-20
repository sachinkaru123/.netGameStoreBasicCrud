using WebApplication1.Data;
using WebApplication1.Endpoints;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connStr = "Data Source=GameStore.db";
        builder.Services.AddSqlite<GameStoreContext> (connStr);
        
        var app = builder.Build();

        app.MapGamesEndPoints();
        
        app.Run();
    }
}