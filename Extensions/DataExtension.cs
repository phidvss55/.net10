using Microsoft.EntityFrameworkCore;
using webapi.Data;

namespace webapi.Extensions;

public static class DataExtension
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
    }
    
}