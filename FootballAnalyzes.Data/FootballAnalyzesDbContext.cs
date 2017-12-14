namespace FootballAnalyzes.Data
{
    using System.Linq;
    using FootballAnalyzes.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class FootballAnalyzesDbContext : IdentityDbContext<User>
    {
        public FootballAnalyzesDbContext(DbContextOptions<FootballAnalyzesDbContext> options)
            : base(options)
        {
        }

        public DbSet<FootballGame> FootballGames { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<GameResult> GameResults { get; set; }
        public DbSet<GameStatistic> GameStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {            
            builder.Entity<FootballGame>()
                .HasOne(g => g.League)
                .WithMany(l => l.Games)
                .HasForeignKey(l => l.LeagueId);

            builder.Entity<Prediction>()
                .HasOne(a => a.Game)
                .WithMany(g => g.Predictions)
                .HasForeignKey(a => a.GameId);

            base.OnModelCreating(builder);
        }
    }
}
