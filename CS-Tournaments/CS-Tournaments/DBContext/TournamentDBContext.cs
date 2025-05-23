using System;
using CS_Tournaments.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CS_Tournaments.DBContext
{
    public class TournamentDBContext : DbContext
    {
        public TournamentDBContext()
        { 
    
        }
        public DbSet<Player> Players { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }            

       
        public TournamentDBContext(DbContextOptions<TournamentDBContext> options)
            : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tournament entity
            base.OnModelCreating(modelBuilder);

            // Player primary key
            modelBuilder.Entity<Player>()
                .HasKey(p => p.Id);

            // Tournament primary key
            modelBuilder.Entity<Tournament>()
                .HasKey(t => t.Id);

            // Self-referencing relationship for nested tournaments
            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.SubTournaments)
                .WithOne(t => t.ParentTournament)
                .HasForeignKey(t => t.ParentTournamentId);

            // Many-to-many relationship between Players and Tournaments
            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Players)
                .WithMany(p => p.Tournaments)
                .UsingEntity(j => j.ToTable("PlayerTournament"));
        }

    }

}
