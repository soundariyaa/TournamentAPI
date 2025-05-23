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
      //  public DbSet<SubTournament> SubTournaments { get; set; }
       
        

       
        public TournamentDBContext(DbContextOptions<TournamentDBContext> options)
            : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.PlayerName).IsRequired();
            });

            // Tournament entity
            modelBuilder.Entity<Tournament>(entity =>
            {
                modelBuilder.Entity<Tournament>()
            .HasMany(t => t.SubTournaments)
            .WithOne(t => t.ParentTournament)
            .HasForeignKey(t => t.ParentTournamentId);

                modelBuilder.Entity<Tournament>()
                    .HasMany(t => t.Players)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("PlayerTournament")); 
                //entity.HasKey(t => t.Id);
                //entity.Property(t => t.Id).ValueGeneratedOnAdd();
                //entity.Property(t => t.Name).IsRequired();

                //// Self-referencing relationship for SubTournaments
                //entity
                //    .HasMany(t => t.SubTournaments)
                //    .WithOne(t => t.ParentTournament)
                //    .HasForeignKey(t => t.ParentTournamentId)
                //    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete if needed

                //// Many-to-Many: Tournament <-> Player
                //entity
                //    .HasMany(t => t.Players)
                //    .WithMany(); // This works fine unless you want to track extra data in the join
            });

            //modelBuilder.Entity<Player>(entity =>
            //{
            //    entity.HasKey(p => new { p.Id, p.PlayerName, p.Age });
            //    entity.Property(p => p.Id).ValueGeneratedOnAdd();
            //});

            //modelBuilder.Entity<Tournament>(entity =>
            //{
            //    entity.HasKey(t => t.Id);
            //    entity.Property(t => t.Id).ValueGeneratedOnAdd();
            //    entity.Property(t => t.Name).IsRequired();                
            //});


            // Defining Primary Key
            //modelBuilder.Entity<Tournament>().HasKey(t => t.Id);

            //// Self-referencing relationship for Sub-Tournaments
            //modelBuilder.Entity<Tournament>()
            //    .HasMany(t => t.SubTournaments)
            //    .WithOne(t => t.ParentTournament)
            //    .HasForeignKey(t => t.ParentTournamentId);

            //// Many-to-Many relationship between Tournaments and Players
            //modelBuilder.Entity<Tournament>()
            //    .HasMany(t => t.Players)
            //    .WithMany(); // Simplified Many-to-Many without extra table



        }

    }

}
