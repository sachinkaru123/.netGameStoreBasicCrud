using System;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options)
 :DbContext(options)
{
    public DbSet<Game> Games =>Set<Game>();

    public DbSet<Genre> Genres =>Set<Genre>();


    public int MyProperty {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Genre>().HasData(
            new {Id =1, Name = "Fighting"},
            new {Id =2, Name = "Action"},
            new {Id =3, Name = "Racing"}


        );
    }
}
