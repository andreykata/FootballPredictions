namespace FootballAnalyzes.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Services.Admin.Models;
    using FootballAnalyzes.Services.Models.Games;
    using FootballAnalyzes.Services.Models.Prediction;

    public class PredictionService : IPredictionService
    {
        private readonly FootballAnalyzesDbContext db;
        private readonly List<string> predictionsName = new List<string>
        {
            ServiceConstants.HTWin,
            ServiceConstants.Draw,
            ServiceConstants.ATWin,
            ServiceConstants.Over8Corners,
            ServiceConstants.Under12Corners
        };

        public PredictionService(FootballAnalyzesDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<AllListingSM> All()
        {
            var result = new List<AllListingSM>();

            foreach (var name in predictionsName)
            {
                var predictCount = this.db
                    .Predictions
                    .Where(p => p.Name == name)
                    .Count();

                result.Add(new AllListingSM{ PredictName = name, GamesCount = predictCount });
            }

            return result;
        }

        public PredictGamesSM PredictGames(string name)
        {
            var pastGames = this.db
                .FootballGames
                .Where(g => g.Predictions.Any(p => p.Name == name) && g.FullTimeResult != null)
                .OrderByDescending(g => g.Predictions.FirstOrDefault(p => p.Name == name).Procent)
                .Take(100)
                .ProjectTo<FootballGamePM>()
                .ToList();

            var wins = pastGames
                 .Where(g => g.Predictions.FirstOrDefault(p => p.Name == name).Result == ServiceConstants.Yes)
                 .Count();

            var winProcent = (wins / (double) pastGames.Count()) * 100;

            return new PredictGamesSM
            {
                Name = name,
                Games = pastGames,
                WinProcent = winProcent
            };
        }
    }
}
