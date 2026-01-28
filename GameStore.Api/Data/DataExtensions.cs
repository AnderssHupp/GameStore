
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    //method to migrate the db in the startup
    public static void MigrateDb(this WebApplication app)
    {
        //Create a scope
        using var scope = app.Services.CreateScope();
        //get the context
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        //migrate
        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        //define the connection string
        var connString = "Data Source=GameStore.db";
        //add the context
        builder.Services.AddSqlite<GameStoreContext>(
            connString,
            //resposible to seed the db when it's created
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Name = "Fighting" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Plataformer" },
                        new Genre { Name = "Racing" },
                        new Genre { Name = "Sports" },
                        new Genre { Name = "Action" }
                    );
                    //Save
                    context.SaveChanges();
                }
            })
        );
    }

}
