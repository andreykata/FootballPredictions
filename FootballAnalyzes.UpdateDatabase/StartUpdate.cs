using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAnalyzes.Data;
using FootballAnalyzes.Data.Models;
using FootballAnalyzes.UpdateDatabase.BindingModels;

namespace FootballAnalyzes.UpdateDatabase
{
    public class StartUpdate
    {
        private readonly FootballAnalyzesDbContext db;

        public StartUpdate(FootballAnalyzesDbContext db)
        {
            this.db = db;

            this.Start();
        }

        public void Start()
        {
            DateTime startCheckDate = DateTime.UtcNow.AddMonths(-6);

            var lastDateGame = this.db
                .FootballGames
                .OrderByDescending(g => g.MatchDate)
                .Select(g => g.MatchDate)
                .FirstOrDefault();

            ReadDataWeb dataWeb = new ReadDataWeb();
            List<FootballGameBM> lastGamesForUpdate = dataWeb.ReadDataFromWeb(lastDateGame);
            // TODO

        }        
    }
}
