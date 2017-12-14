namespace FootballAnalyzes.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Services.Models.Games;

    using static FootballAnalyzes.Services.ServiceConstants;

    public class GameService : IGameService
    {
        private readonly FootballAnalyzesDbContext db;

        public GameService(FootballAnalyzesDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<FootballGameSM> All(int page = 1)
        {
            return this.db
                .FootballGames
                .Where(g => g.FullTimeResult != null)
                .OrderByDescending(g => g.MatchDate.Date)
                .ThenBy(g => g.Id)
                .Skip((page - 1) * GamesPageSize)
                .Take(GamesPageSize)
                .ProjectTo<FootballGameSM>()
                .ToList();
        }

        public IEnumerable<FootballGameSM> Next(int page = 1)
        {
            return this.db
                .FootballGames
                .Where(g => g.FullTimeResult == null)
                .OrderByDescending(g => g.Id)
                .Skip((page - 1) * GamesPageSize)
                .Take(GamesPageSize)
                .ProjectTo<FootballGameSM>()
                .ToList();
        }

        public int TotalGamesCount() => this.db.FootballGames.Where(g => g.FullTimeResult != null).Count();

        public int TotalNextGamesCount() => this.db.FootballGames.Where(g => g.FullTimeResult == null).Count();
    }
}
