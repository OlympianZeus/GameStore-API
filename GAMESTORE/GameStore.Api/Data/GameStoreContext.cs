using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options)
    : DbContext(options)//the DbContextOptions of type GameStoreContext which has options as the name
{
    public DbSet<Game> Games => Set<Game>();//Dbset of the type of the entity(Game)
    //A Dbset is an object that can be used to query and save the instance of the Game
    public DbSet<Genre> Genres => Set<Genre>();

    //this method executes every time the migrations are executed
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Fighting" },
            new { Id = 2, Name = "RolePlaying" },
            new { Id = 3, Name = "Sports" },
            new { Id = 4, Name = "Racing" },
            new { Id = 5, Name = "Kids and Familly" }
        );
    }
}

