using System;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data;

public static class DataExtensions
{
    public static void MigrationDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
        
    }
}
