using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Data;

public class FootballLeagueDbContext : DbContext
{
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    //base.OnConfiguring(optionsBuilder); //cip...16. default not needed.
    //using sqlserver
    //optionsBuilder.UseSqlServer("Data Source=localhost,1448;Initial Catalog=FootballLeague_EFCore;Encrypt=False;user id=sa;password=Str0ngPa$$w0rd;"); //to be tested
    optionsBuilder.UseSqlite($"Data Source=FootballLeague_EFCore.db");
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    //base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Team>().HasData(
      new Team { Id = 1, Name = "Tivoli Gardens FC", CreatedDate = new DateTime(2025, 5, 9, 18, 0, 0) }, //hard-coding due to migration errors. DateTimeOffset.UtcNow.DateTime
      new Team { Id = 2, Name = "Waterhouse FC", CreatedDate = new DateTime(2025, 5, 9, 18, 0, 0) },
      new Team { Id = 3, Name = "Humble Lions FC", CreatedDate = new DateTime(2025, 5, 9, 18, 0, 0) }
    );

  }

  public DbSet<Team> Teams { get; set; }
  public DbSet<Coach> Coaches { get; set; }
  //public DbSet<League> Leagues { get; set; }
  //public DbSet<Match> Matches { get; set; }
}
