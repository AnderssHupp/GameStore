
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
        var connString = builder.Configuration.GetConnectionString("GameStore");
        //builder.Services.AddScoped<GameStoreContext>();
        //DbContext has a Scoped service lifetime because:
        // 1. Garante que uma nova instancia de DbContext é criada por requisição
        // 2. Db conexões são limitadas e um recurso caro
        // 3. DbContext não é thread-safe. Scoped avoids to concurrency issues.
        // 4. Torna mais facil de generenciar transações e garantir data consistency
        // 5. Reuso da instancia de DbContext pode aumentar o uso de memória      





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
