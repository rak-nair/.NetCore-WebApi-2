using FeedbackAPI.Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace FeedbackAPI.Data
{
    public class FeedbackDbContext : DbContext
    {
        public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options) :
            base(options)
        {
            Database.Migrate();
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameSession>(g =>
            {
                g.HasOne<Game>()
                    .WithMany()
                    .HasForeignKey(s => s.GameID);
            });

            modelBuilder.Entity<Feedback>(b =>
            {
                b.HasOne<Player>()
                    .WithMany()
                    .HasForeignKey(c => c.PlayerID);

                b.HasOne<GameSession>()
                    .WithMany()
                    .HasForeignKey(c => c.GameSessionID);
            });


            modelBuilder.Entity<Player>().HasData(
                new Player[4]
                {
                    new Player(){ ScreenName = "Geralt", PlayerID = 1},
                    new Player(){ ScreenName = "Marcus", PlayerID = 2},
                    new Player(){ ScreenName = "Watcher", PlayerID = 3},
                    new Player(){ ScreenName = "Bradford", PlayerID = 4}

                });

            modelBuilder.Entity<Game>().HasData(
                new Game[4]
                {
                    new Game(){ GameName = "The Witcher", GameID = 1, Publisher = "CD Project", ReleaseYear = 2017},
                    new Game(){ GameName = "Gears Of War", GameID = 2, Publisher = "Microsoft", ReleaseYear = 2009},
                    new Game(){ GameName = "Pillars Of Eternity", GameID = 3, Publisher = "Paradox", ReleaseYear = 2015 },
                    new Game(){ GameName = "XCOM2", GameID = 4, Publisher = "2K", ReleaseYear = 2017 }
                });

            DateTime currentDateTime = DateTime.Now;
            modelBuilder.Entity<GameSession>().HasData(
                new GameSession[6]
                {
                    new GameSession(){ GameSessionID = 1, GameID = 1, SessionStartTime = currentDateTime.AddDays(-10).AddHours(-5) , SessionEndTime = currentDateTime.AddDays(-10)},
                    new GameSession(){ GameSessionID = 2, GameID = 1, SessionStartTime = currentDateTime.AddDays(-20) , SessionEndTime = currentDateTime.AddDays(-19)},
                    new GameSession(){ GameSessionID = 3, GameID = 1, SessionStartTime = currentDateTime.AddDays(-30).AddHours(-1) , SessionEndTime = currentDateTime.AddDays(-28).AddHours(1)},
                    new GameSession(){ GameSessionID = 4, GameID = 2, SessionStartTime = currentDateTime.AddDays(-65).AddHours(-1) , SessionEndTime = currentDateTime.AddDays(-65).AddHours(10).AddMinutes(20)},
                    new GameSession(){ GameSessionID = 5, GameID = 2, SessionStartTime = currentDateTime.AddDays(-60).AddHours(-5).AddMinutes(20) , SessionEndTime = currentDateTime.AddDays(-60).AddHours(1).AddMinutes(19)},
                    new GameSession(){ GameSessionID = 6, GameID = 3, SessionStartTime = currentDateTime.AddDays(-3).AddHours(-10).AddMinutes(5) , SessionEndTime = currentDateTime.AddDays(-3).AddHours(3)},
                });
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Feedback> Feedback { get; set; }

    }
}
