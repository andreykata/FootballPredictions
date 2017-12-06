using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
using FootballAnalyzes.Data;
using FootballAnalyzes.Services.Models.Games;
using FootballAnalyzes.UpdateDatabase;

namespace FootballAnalyzes.Services.Implementations
{
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
                .OrderByDescending(g => g.Id)
                .ProjectTo<FootballGameSM>()
                .ToList();
        }

        public void UpdateDb()
        {
            var updateDb = new StartUpdate(this.db);
        }
    }
}
