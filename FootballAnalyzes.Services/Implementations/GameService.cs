namespace FootballAnalyzes.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Data.Models;
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
                .OrderByDescending(g => g.MatchDate.Date)
                .ThenBy(g => g.Id)
                .Skip((page - 1) * GamesPageSize)
                .Take(GamesPageSize)
                .ProjectTo<FootballGameSM>()
                .ToList();
        }

        public IEnumerable<FootballGameSM> WithoutResult(DateTime date, int page = 1)
        {
            return this.db
                .FootballGames
                .Where(g => g.FullTimeResult == null && g.MatchDate.Date == date.Date)
                .OrderByDescending(g => g.Id)
                .Skip((page - 1) * GamesPageSize)
                .Take(GamesPageSize)
                .ProjectTo<FootballGameSM>()
                .ToList();
        }

        public IEnumerable<ByDateSM> GroupByDate(int page = 1)
        {
            var firstDate = this.db.FootballGames.FirstOrDefault().MatchDate.Date;
            var lastDate = this.db.FootballGames.OrderBy(g => g.MatchDate.Date).LastOrDefault().MatchDate.Date;
            List<DateTime> allDates = new List<DateTime>();

            for (var i = firstDate; i <= lastDate; i = i.AddDays(1))
            {
                allDates.Add(i);
            }

            var pageDates = allDates
                .OrderByDescending(d => d.Date)
                .Skip((page - 1) * GamesByDatePageSize)
                .Take(GamesByDatePageSize)
                .ToList();

            var gamesList = this.db
                .FootballGames
                .Where(g => g.MatchDate.Date >= pageDates.LastOrDefault() && g.MatchDate.Date <= pageDates.FirstOrDefault())
                .Select(g => new
                {
                    MatchDate = g.MatchDate,
                    FullTimeResult = g.FullTimeResult
                })
                .ToList();

            var gamesByDate = gamesList
                .GroupBy(g => g.MatchDate.Date)
                .Select(gr => new ByDateSM
                {
                    GamesDate = gr.Key,
                    GamesWithResultCount = gr.Where(g => g.FullTimeResult != null).Count(),
                    GamesWithoutResultCount = gr.Where(g => g.FullTimeResult == null).Count()
                })
                .OrderByDescending(gr => gr.GamesDate)
                .ToList();

            var result = new List<ByDateSM>();
            foreach (var date in pageDates)
            {
                result.Add(new ByDateSM
                {
                    GamesDate = date.Date,
                    GamesWithResultCount = gamesList.Where(g => g.MatchDate.Date == date.Date && g.FullTimeResult != null).Count(),
                    GamesWithoutResultCount = gamesList.Where(g => g.MatchDate.Date == date.Date && g.FullTimeResult == null).Count()
                });
            }
            
            return result;
        }

        public int TotalGamesCount() => this.db.FootballGames.Where(g => g.FullTimeResult != null).Count();

        public int TotalNextGamesCount() => this.db.FootballGames.Where(g => g.FullTimeResult == null).Count();

        public int TotalByDateCount() => this.db.FootballGames.GroupBy(g => g.MatchDate.Date).Count();

        public int TotalCurrnetDateGamesCount(DateTime date) => this.db.FootballGames.Where(g => g.FullTimeResult == null && g.MatchDate.Date == date.Date).Count();

        public EditGameSM FindGameForEdit(int id)
        {
            return this.db
                .FootballGames
                .Where(g => g.Id == id)
                .ProjectTo<EditGameSM>()
                .FirstOrDefault();
        }

        public bool EditGame(int id, int homeTeamGoals, int awayTeamGoals)
        {
            var game = this.db
                .FootballGames
                .Where(g => g.Id == id)
                .FirstOrDefault();

            if (game == null)
            {
                return false;
            }

            GameResult fullTime = new GameResult
            {
                HomeTeamGoals = homeTeamGoals,
                AwayTeamGoals = awayTeamGoals,
                Result = homeTeamGoals > awayTeamGoals ? ResultEnum.H : (homeTeamGoals < awayTeamGoals ? ResultEnum.A : ResultEnum.D)
            };

            game.FullTimeResult = fullTime;
            this.db.SaveChanges();

            return true;
        }
    }
}
