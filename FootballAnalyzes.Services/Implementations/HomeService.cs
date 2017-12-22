namespace FootballAnalyzes.Services.Implementations
{
    using System.Linq;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Services.Models;

    public class HomeService : IHomeService
    {
        private readonly FootballAnalyzesDbContext db;

        public HomeService(FootballAnalyzesDbContext db)
        {
            this.db = db;
        }

        public HomeIndexSM HomeInfo()
        {
            int gamesCount = this.db.FootballGames.Where(g => g.FullTimeResult != null).Count();
            int countries = this.db
                .Leagues
                .GroupBy(l => l.Country)
                .Count();
            int leagues = this.db
                .Leagues
                .GroupBy(l => l.Country + l.Name)
                .Count();
            int teams = this.db.Teams.Count();

            return new HomeIndexSM
            {
                Games = gamesCount,
                Countries = countries,
                Leagues = leagues,
                Teams = teams
            };
        }
    }
}
