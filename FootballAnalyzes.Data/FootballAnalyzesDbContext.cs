namespace FootballAnalyzes.Web.Data
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

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }            

            builder.Entity<FootballGame>()
                .HasOne(g => g.HomeTeam)
                .WithMany()
                .HasForeignKey(g => g.HomeTeamId);

            builder.Entity<FootballGame>()
                .HasOne(g => g.AwayTeam)
                .WithMany()
                .HasForeignKey(g => g.AwayTeamId);

            builder.Entity<FootballGame>()
                .HasOne(g => g.League)
                .WithMany(l => l.Games)
                .HasForeignKey(l => l.LeagueId);

            builder.Entity<Prediction>()
                .HasOne(a => a.Game)
                .WithMany(g => g.Predictions)
                .HasForeignKey(a => a.GameId);

            builder.Entity<FootballGame>()
               .HasOne(g => g.FullTimeResult)
               .WithOne()
               .HasForeignKey<FootballGame>(g => g.FullTimeResultId);

            builder.Entity<FootballGame>()
               .HasOne(g => g.FirstHalfResult)
               .WithOne()
               .HasForeignKey<FootballGame>(g => g.FirstHalfResultId);

            base.OnModelCreating(builder);
        }
    }
}
