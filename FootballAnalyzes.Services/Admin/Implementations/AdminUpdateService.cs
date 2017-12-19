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
    using FootballAnalyzes.UpdateDatabase;

    using static FootballAnalyzes.Services.ServiceConstants;

    public class AdminUpdateService : IAdminUpdateService
    {
        private readonly FootballAnalyzesDbContext db;

        private List<FootballGamePM> games;
        private Dictionary<string, TeamPM> teams;
        private SortedDictionary<string, LeaguePM> leagues;

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

        public void MakePredictionToOldGames()
        {
            DateTime startDate = DateTime.Now.AddMonths(ReturnMonthsForOldGames);

            var allGames = this.db
                .FootballGames
                .Where(g => g.FullTimeResult != null)
                .ToList();

            var gamesForPredict = this.db
                .FootballGames
                .Where(g => g.MatchDate >= startDate && g.FullTimeResult != null && g.Predictions.Count() == 0)
                .ProjectTo<FootballGameSM>()
                .ToList();

            int count = 1;
            foreach (var game in gamesForPredict)
            {
                var homeTeamGames = allGames
                    .Where(g => g.MatchDate.Date < game.MatchDate.Date && (g.HomeTeam.Id == game.HomeTeam.Id || g.AwayTeam.Id == game.HomeTeam.Id) && g.FullTimeResult != null)
                    .ToList();

                var awayTeamGames = allGames
                   .Where(g => g.MatchDate.Date < game.MatchDate.Date && (g.HomeTeam.Id == game.AwayTeam.Id || g.AwayTeam.Id == game.AwayTeam.Id) && g.FullTimeResult != null)
                   .ToList();

                var gamesBetweenBothTeams = homeTeamGames
                    .Where(g => g.HomeTeam.Id == game.AwayTeam.Id || g.AwayTeam.Id == game.AwayTeam.Id)
                    .ToList();
                Console.WriteLine(count++);
            }


        }
    }
}
