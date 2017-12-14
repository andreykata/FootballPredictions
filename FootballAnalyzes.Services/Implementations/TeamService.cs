namespace FootballAnalyzes.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Services.Models.Games;
    using FootballAnalyzes.Services.Models.Teams;

    using static FootballAnalyzes.Services.ServiceConstants;

    public class TeamService : ITeamService
    {
        private readonly FootballAnalyzesDbContext db;
        private readonly IGameService games;

        public TeamService(FootballAnalyzesDbContext db, IGameService games)
        {
            this.db = db;
            this.games = games;
        }

        public IEnumerable<TeamListingSM> All(int page = 1)
        {
            return this.db
                .Teams
                .OrderBy(t => t.Name)
                .Skip((page - 1) * GamesPageSize)
                .Take(GamesPageSize)
                .Select(t => new TeamListingSM
                {
                    Id = t.Id,
                    Name = t.Name,
                    UniqueName = t.UniqueName,
                    TeamGamesCount = this.db
                        .FootballGames
                        .Where(g => g.HomeTeamId == t.Id || g.AwayTeamId == t.Id)
                        .Count()
                })
                .ToList();
        }

        public int TotalTeamsCount() => this.db.Teams.Count();
    }
}
