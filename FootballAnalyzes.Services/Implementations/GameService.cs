namespace FootballAnalyzes.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Services.Models.Games;

    public class GameService : IGameService
    {
        private readonly FootballAnalyzesDbContext db;

        public GameService(FootballAnalyzesDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<FootballGameSM> All()
        {
            return this.db
                .FootballGames
                .Where(g => g.FullTimeResult != null)
                .OrderByDescending(g => g.Id)
                .ProjectTo<FootballGameSM>()
                .ToList();
        }        
    }
}
