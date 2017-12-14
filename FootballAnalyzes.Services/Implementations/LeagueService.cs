namespace FootballAnalyzes.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using FootballAnalyzes.Data;
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

        public int TotalLeaguesCount() => this.db.Leagues.Count();
    }
}
