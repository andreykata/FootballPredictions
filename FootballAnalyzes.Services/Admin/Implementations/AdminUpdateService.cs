namespace FootballAnalyzes.Services.Admin.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Data.Models;
    using FootballAnalyzes.Services.Admin.Models;
    using FootballAnalyzes.Services.Models.Games;
    using FootballAnalyzes.Services.Predictions.Analyzes;
    using FootballAnalyzes.UpdateDatabase;

    using static FootballAnalyzes.Services.ServiceConstants;

    public class AdminUpdateService : IAdminUpdateService
    {
        private readonly FootballAnalyzesDbContext db;
        
        public AdminUpdateService(FootballAnalyzesDbContext db)
        {
            this.db = db;
        }

        public bool DeleteGamesByDate(string date)
        {
            DateTime deleteDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            List<FootballGame> deleteGames = this.db
                .FootballGames
                .Where(g => g.MatchDate.Date == deleteDate)
                .ToList();

            if (deleteGames == null)
            {
                return false;
            }

            foreach (var game in deleteGames)
            {
                this.db.FootballGames.Remove(game);
            }

            this.db.SaveChanges();

            return true;
        }

        public int DeleteGamesByDateCount(string date)
        {
            DateTime deleteDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            return this.db
                .FootballGames
                .Where(g => g.MatchDate.Date == deleteDate)
                .Count();
        }       

        public string UpdateDatesInfo()
        {
            DateTime lastDateGame = this.FindOldestPossibleDateForUpdate();
            DateTime endDate = DateTime.Now.AddDays(-2).Date;
            string dates = 
                lastDateGame == endDate ? "none" : 
                (lastDateGame.AddDays(1).Date == endDate 
                    ? lastDateGame.AddDays(1).ToString(DateStringFormat) 
                    : lastDateGame.AddDays(1).ToString(DateStringFormat) + " - " + endDate.ToString(DateStringFormat));

            return $"Dates for update: {dates}";
        }

        public string UpdateDb(DateTime nextGamesDate)
        {
            DateTime lastDateGame = this.FindOldestPossibleDateForUpdate();
            DateTime endDate = DateTime.Now.AddDays(-2).Date;
            var updateDb = new StartUpdate(this.db);
           
            var gamesBeforeUpdate = this.db
                .FootballGames
                .Where(g => g.MatchDate.Date >= lastDateGame && g.MatchDate.Date <= endDate && g.FullTimeResult != null)
                .Count();
            int newGamesBeforeUpdateCount = this.db.FootballGames.Where(g => g.MatchDate.Date == nextGamesDate).Count();

            updateDb.MakeAnalyzesForNextGames(lastDateGame, endDate, nextGamesDate);

            var gamesAfterUpdate = this.db
                .FootballGames
                .Where(g => g.MatchDate.Date >= lastDateGame && g.MatchDate.Date <= endDate && g.FullTimeResult != null)
                .Count();

            int updatedGamesCount = gamesAfterUpdate - gamesBeforeUpdate;
            int newGamesCount = this.db.FootballGames.Where(g => g.MatchDate.Date == nextGamesDate).Count();

            return $"Updated old games : {updatedGamesCount}," +
                $"Add new games: {newGamesCount - newGamesBeforeUpdateCount}";
        }

        public string UpdateOldGames(DateTime startDate, DateTime endDate)
        {
            var updateDb = new StartUpdate(this.db);

            var gamesBeforeUpdate = this.db
                .FootballGames
                .Where(g => g.MatchDate.Date >= startDate && g.MatchDate.Date <= endDate && g.FullTimeResult != null)
                .Count();

            updateDb.UpdateOldGames(startDate, endDate);

            var gamesAfterUpdate = this.db
                .FootballGames
                .Where(g => g.MatchDate.Date >= startDate && g.MatchDate.Date <= endDate && g.FullTimeResult != null)
                .Count();

            int updatedGamesCount = gamesAfterUpdate - gamesBeforeUpdate;

            return $"Updated old games : {updatedGamesCount}";
        }

        private DateTime FindOldestPossibleDateForUpdate()
        {
            var minDateForNewResults = DateTime.Now.AddDays(-2);

            var lastDateGame = this.db
                .FootballGames
                .Where(g => g.MatchDate.Date <= minDateForNewResults.Date && g.FullTimeResult != null)
                .OrderByDescending(g => g.MatchDate)
                .Select(g => g.MatchDate)
                .FirstOrDefault();

            return lastDateGame.Date;
        }

        public string MakePredictionToOldGames()
        {
            DateTime startDate = DateTime.Now.AddMonths(-1);

            var gamesForPredict = this.db
                .FootballGames
                .Where(g => g.MatchDate >= startDate && g.FullTimeResult != null && g.Predictions.Count == 0)
                .ProjectTo<FootballGamePM>()
                .ToList();

            int predictionCount = this.db.Predictions.Count();

            CreatePredictions(gamesForPredict, PastGames);

            int predictionsAfterCount = this.db.Predictions.Count();

            return $"Successfull added {predictionsAfterCount - predictionCount} predictions to {gamesForPredict.Count()}";
        }

        public string MakePredictionToNewGames(DateTime nextGamesDate)
        {
            var gamesForPredict = this.db
                .FootballGames
                .Where(g => g.MatchDate.Date == nextGamesDate.Date && g.Predictions.Count == 0)
                .ProjectTo<FootballGamePM>()
                .ToList();

            int predictionCount = this.db.Predictions.Count();

            CreatePredictions(gamesForPredict, ServiceConstants.NextGames);

            int predictionsAfterCount = this.db.Predictions.Count();

            return $"Successfull added {predictionsAfterCount - predictionCount} predictions to {gamesForPredict.Count()}";
        }

        private void CreatePredictions(List<FootballGamePM> gamesForPredict, string typeGames)
        {
            int count = 0;
            foreach (var game in gamesForPredict)
            {
                var homeTeamGames = this.db
                    .FootballGames
                    .Where(g => (g.HomeTeam.Id == game.HomeTeam.Id || g.AwayTeam.Id == game.HomeTeam.Id) && g.MatchDate.Date < game.MatchDate.Date && g.FullTimeResult != null)
                    .ProjectTo<FootballGameSM>()
                    .ToList();

                var awayTeamGames = this.db
                    .FootballGames
                   .Where(g => (g.HomeTeam.Id == game.AwayTeam.Id || g.AwayTeam.Id == game.AwayTeam.Id) && g.MatchDate.Date < game.MatchDate.Date && g.FullTimeResult != null)
                   .ProjectTo<FootballGameSM>()
                   .ToList();

                var gamesBetweenBothTeams = homeTeamGames
                    .Where(g => (g.HomeTeam.Id == game.AwayTeam.Id || g.AwayTeam.Id == game.AwayTeam.Id))
                    .ToList();

                // Add all prediction analyzes to the game
                AddAllAnalyzesToGame(game, gamesBetweenBothTeams, homeTeamGames, awayTeamGames, typeGames);

                if (typeGames == PastGames)
                {
                    List<Tuple<string, bool, bool>> predictions = new List<Tuple<string, bool, bool>>()
                    {
                        new Tuple<string, bool, bool>(HTWin, game.FullTimeResult.Result == ResultEnum.H, false),
                        new Tuple<string, bool, bool>(ATWin, game.FullTimeResult.Result == ResultEnum.A, false),
                        new Tuple<string, bool, bool>(Draw, game.FullTimeResult.Result == ResultEnum.D, false)
                    };

                    if (game.GameStatistic != null)
                    {
                        predictions.Add(new Tuple<string, bool, bool>(Over8Corners, (game.GameStatistic.HomeTeamCorners + game.GameStatistic.AwayTeamCorners > 8), game.GameStatistic == null));
                        predictions.Add(new Tuple<string, bool, bool>(Under12Corners, (game.GameStatistic.HomeTeamCorners + game.GameStatistic.AwayTeamCorners < 12), game.GameStatistic == null));
                    }

                    foreach (var predict in predictions)
                    {
                        MakeAnalysis(game, predict.Item1, predict.Item2, predict.Item3);
                    }
                }

                foreach (var predict in game.Predictions)
                {
                    Prediction PredictDb = new Prediction
                    {
                        GameId = game.Id,
                        Name = predict.Name,
                        Procent = predict.Procent,
                        Result = predict.Result
                    };

                    this.db.AddRange(PredictDb);
                }

                Console.WriteLine(count++);
            }

            this.db.SaveChanges();
        }

        private void AddAllAnalyzesToGame(FootballGamePM game, List<FootballGameSM> gamesBetweenBothTeams, List<FootballGameSM> homeTeamGames, List<FootballGameSM> awayTeamGames, string typeGames)
        {
            Result1X2 result1X2 = new Result1X2(game, gamesBetweenBothTeams, homeTeamGames, awayTeamGames, 40, 20, 10, 5);

            if (typeGames == PastGames && game.GameStatistic != null)
            {
                Corners corners = new Corners(game, gamesBetweenBothTeams, homeTeamGames, awayTeamGames, 10, 10, 0, 0);
            }
            else if (typeGames == NextGames)
            {
                Corners corners = new Corners(game, gamesBetweenBothTeams, homeTeamGames, awayTeamGames, 10, 10, 0, 0);
            }
        }

        private void MakeAnalysis(FootballGamePM game, string prediction, bool winCondition, bool deleteCondition)
        {
            if (!game.Predictions.Any(p => p.Name == prediction))
            {
                return;
            }
            
            PredictionSM predict = game.Predictions.Where(p => p.Name == prediction).FirstOrDefault();

            // That is only for pased games. The new games havn't play yet and they don't have statistics.
            if (deleteCondition)
            {
                game.Predictions.Remove(predict);
                return;
            }

            if (winCondition)
            {
                predict.Result = ServiceConstants.Yes;
            }
            else
            {
                predict.Result = ServiceConstants.No;
            }
        }
    }
}
