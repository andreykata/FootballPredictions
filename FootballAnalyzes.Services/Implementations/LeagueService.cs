namespace FootballAnalyzes.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Services.Admin.Models;
    using FootballAnalyzes.Services.Models;

    using static FootballAnalyzes.Services.ServiceConstants;

    public class LeagueService : ILeagueService
    {
        private readonly FootballAnalyzesDbContext db;

        public LeagueService(FootballAnalyzesDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<LeagueListingSM> All(int page = 1)
        {
            var leagues = this.db
                .Leagues
                .OrderBy(l => l.Name)
                .Skip((page - 1) * GamesPageSize)
                .Take(GamesPageSize)
                .ProjectTo<LeagueListingSM>()
                .ToList();

            return leagues;
        }

        public string NameById(int leagueId) => this.db.Leagues.Find(leagueId).Name;

        public IEnumerable<FootballGamePM> LeagueGames(int leagueId)
        {
            return this.db
                .FootballGames
                .Where(g => g.LeagueId == leagueId && (g.FullTimeResult != null || g.MatchDate.Date >= DateTime.Now.Date))
                .OrderByDescending(g => g.MatchDate)
                .ThenBy(g => g.Id)
                .ProjectTo<FootballGamePM>()
                .ToList();
        }

        public int TotalLeaguesCount() => this.db.Leagues.Count();
    }
}
