﻿namespace FootballAnalyzes.UpdateDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Data.Models;
    using FootballAnalyzes.UpdateDatabase.BindingModels;

    public class StartUpdate
    {
        private readonly FootballAnalyzesDbContext db;

        private List<FootballGameBM> lastGamesForUpdate;
        private List<FootballGameBM> nextGames;

        public StartUpdate(FootballAnalyzesDbContext db)
        {
            this.db = db;

            this.lastGamesForUpdate = new List<FootballGameBM>();
            this.nextGames = new List<FootballGameBM>();            
        }
        
        public void MakeAnalyzesForNextGames(DateTime lastDateGame, DateTime endDate, DateTime nextGamesDate)
        {
            // Read old games without results from soccerway.com
            ReadDataWeb dataWeb = new ReadDataWeb();
            this.lastGamesForUpdate = dataWeb.ReadDataFromWeb(lastDateGame, endDate);

            AddLastGamesForUpdateToDb(this.lastGamesForUpdate);

            // Read next games from soccerway.com
            ReadNextGames nextGamesDataWeb = new ReadNextGames();

            var existGamesFromNextDate = this.db
                .FootballGames
                .Any(g => g.MatchDate.Date == nextGamesDate.Date);

            if (!existGamesFromNextDate)
            {
                this.nextGames = nextGamesDataWeb.ReadDataFromWeb(nextGamesDate);

                AddLastGamesForUpdateToDb(this.nextGames);
            }
        }

        public void UpdateOldGames(DateTime startDate, DateTime endDate)
        {
            ReadDataWeb dataWeb = new ReadDataWeb();
            startDate = startDate.AddDays(-1);
            this.lastGamesForUpdate = dataWeb.ReadDataFromWeb(startDate, endDate);

            AddLastGamesForUpdateToDb(this.lastGamesForUpdate);
        }

        public void SeedOldGames()
        {
            var games = Seed.ReadGamesFromFile();

            this.AddOldGamesToDb(games);
        }

        private void AddLastGamesForUpdateToDb(List<FootballGameBM> lastGamesForUpdate)
        {
            foreach (var game in lastGamesForUpdate)
            {
                var existGame = this.db
                    .FootballGames
                    .Where(g =>
                        (g.MatchDate.Date == game.MatchDate.Date) &&
                        (g.HomeTeam.UniqueName == game.HomeTeam.UniqueName) &&
                        (g.AwayTeam.UniqueName == game.AwayTeam.UniqueName) &&
                        (g.League.Name == game.League.Name))
                    .FirstOrDefault();

                if (existGame == null)
                {
                    // Add home team to the DB and to current game
                    var homeTeam = this.db
                        .Teams
                        .Where(t => t.UniqueName == game.HomeTeam.UniqueName)
                        .FirstOrDefault();

                    if (homeTeam == null)
                    {
                        homeTeam = new Team
                        {
                            Name = game.HomeTeam.Name,
                            UniqueName = game.HomeTeam.UniqueName
                        };

                        this.db.Add(homeTeam);
                        this.db.SaveChanges();
                    }

                    // Add away team to the DB and to current game
                    var awayTeam = this.db
                        .Teams
                        .Where(t => t.UniqueName == game.AwayTeam.UniqueName)
                        .FirstOrDefault();

                    if (awayTeam == null)
                    {
                        awayTeam = new Team
                        {
                            Name = game.AwayTeam.Name,
                            UniqueName = game.AwayTeam.UniqueName
                        };

                        this.db.Add(awayTeam);
                        this.db.SaveChanges();
                    }


                    // Add league to the DB and to current game
                    var league = this.db
                        .Leagues
                        .Where(l => l.UniqueName == game.League.UniqueName)
                        .FirstOrDefault();

                    if (league == null)
                    {
                        league = new League
                        {
                            Name = game.League.Name,
                            Country = game.League.Country,
                            Year = game.League.Year,
                            Stage = game.League.Stage,
                            Type = game.League.Type,
                            UniqueName = game.League.UniqueName
                        };

                        this.db.Add(league);
                        this.db.SaveChanges();
                    }

                    // Add current game
                    existGame = new FootballGame
                    {
                        MatchDate = game.MatchDate,
                        LeagueId = league.Id,
                        League = league,
                        HomeTeamId = homeTeam.Id,
                        HomeTeam = homeTeam,
                        AwayTeamId = awayTeam.Id,
                        AwayTeam = awayTeam
                    };

                    this.db.Add(existGame);
                    this.db.SaveChanges();
                    
                    league.Games.Add(existGame);

                    this.db.SaveChanges();
                }

                // Add full time result to the DB and to current game
                if (game.FullTimeResult != null)
                {
                    GameResult fullTimeResult = new GameResult
                    {
                        Result = game.FullTimeResult.Result,
                        HomeTeamGoals = game.FullTimeResult.HomeTeamGoals,
                        AwayTeamGoals = game.FullTimeResult.AwayTeamGoals
                    };

                    this.db.Add(fullTimeResult);
                    this.db.SaveChanges();

                    existGame.FullTimeResult = fullTimeResult;
                    existGame.FullTimeResultId = fullTimeResult.Id;
                }

                // Add first time result to the DB and to current game
                if (game.FirstHalfResult != null)
                {
                    GameResult firstHalf = new GameResult
                    {
                        Result = game.FirstHalfResult.Result,
                        HomeTeamGoals = game.FirstHalfResult.HomeTeamGoals,
                        AwayTeamGoals = game.FirstHalfResult.AwayTeamGoals
                    };

                    this.db.Add(firstHalf);
                    this.db.SaveChanges();

                    existGame.FirstHalfResult = firstHalf;
                    existGame.FirstHalfResultId = firstHalf.Id;
                }

                // Add game statistic to the DB and to current game
                if (game.GameStatistic != null)
                {
                    GameStatistic gameStatistic = new GameStatistic
                    {
                        HomeTeamCorners = game.GameStatistic.HomeTeamCorners,
                        AwayTeamCorners = game.GameStatistic.AwayTeamCorners,
                        HomeTeamShotsOnTarget = game.GameStatistic.HomeTeamShotsOnTarget,
                        AwayTeamShotsOnTarget = game.GameStatistic.AwayTeamShotsOnTarget,
                        HomeTeamShotsWide = game.GameStatistic.HomeTeamShotsWide,
                        AwayTeamShotsWide = game.GameStatistic.AwayTeamShotsWide,
                        HomeTeamFouls = game.GameStatistic.HomeTeamFouls,
                        AwayTeamFouls = game.GameStatistic.AwayTeamFouls,
                        HomeTeamOffsides = game.GameStatistic.HomeTeamOffsides,
                        AwayTeamOffsides = game.GameStatistic.AwayTeamOffsides
                    };

                    this.db.Add(gameStatistic);
                    this.db.SaveChanges();

                    existGame.GameStatistic = gameStatistic;
                    existGame.GameStatisticId = gameStatistic.Id;
                }

                this.db.SaveChanges();
            }
        }

        private void AddOldGamesToDb(List<FootballGameBM> lastGamesForUpdate)
        {
            foreach (var game in lastGamesForUpdate)
            {
                this.db.ChangeTracker.AutoDetectChangesEnabled = false;

                // Add home team to the DB and to current game
                var homeTeam = this.db
                    .Teams
                    .Where(t => t.UniqueName == game.HomeTeam.UniqueName)
                    .FirstOrDefault();

                if (homeTeam == null)
                {
                    homeTeam = new Team
                    {
                        Name = game.HomeTeam.Name,
                        UniqueName = game.HomeTeam.UniqueName
                    };

                    this.db.AddRange(homeTeam);
                    this.db.SaveChanges();
                }

                // Add away team to the DB and to current game
                var awayTeam = this.db
                    .Teams
                    .Where(t => t.UniqueName == game.AwayTeam.UniqueName)
                    .FirstOrDefault();

                if (awayTeam == null)
                {
                    awayTeam = new Team
                    {
                        Name = game.AwayTeam.Name,
                        UniqueName = game.AwayTeam.UniqueName
                    };

                    this.db.AddRange(awayTeam);
                    this.db.SaveChanges();
                }


                // Add league to the DB and to current game
                var league = this.db
                    .Leagues
                    .Where(l => l.UniqueName == game.League.UniqueName)
                    .FirstOrDefault();

                if (league == null)
                {
                    league = new League
                    {
                        Name = game.League.Name,
                        Country = game.League.Country,
                        Year = game.League.Year,
                        Stage = game.League.Stage,
                        Type = game.League.Type,
                        UniqueName = game.League.UniqueName
                    };

                    this.db.AddRange(league);
                    this.db.SaveChanges();
                }

                GameResult fullTimeResult = null;
                if (game.FullTimeResult != null)
                {
                    fullTimeResult = new GameResult
                    {
                        Result = game.FullTimeResult.Result,
                        HomeTeamGoals = game.FullTimeResult.HomeTeamGoals,
                        AwayTeamGoals = game.FullTimeResult.AwayTeamGoals
                    };

                    this.db.AddRange(fullTimeResult);
                    this.db.SaveChanges();
                }

                GameResult firstHalf = null;
                // Add first time result to the DB and to current game
                if (game.FirstHalfResult != null)
                {
                    firstHalf = new GameResult
                    {
                        Result = game.FirstHalfResult.Result,
                        HomeTeamGoals = game.FirstHalfResult.HomeTeamGoals,
                        AwayTeamGoals = game.FirstHalfResult.AwayTeamGoals
                    };

                    this.db.AddRange(firstHalf);
                    this.db.SaveChanges();
                }

                GameStatistic gameStatistic = null;
                // Add game statistic to the DB and to current game
                if (game.GameStatistic != null)
                {
                    gameStatistic = new GameStatistic
                    {
                        HomeTeamCorners = game.GameStatistic.HomeTeamCorners,
                        AwayTeamCorners = game.GameStatistic.AwayTeamCorners,
                        HomeTeamShotsOnTarget = game.GameStatistic.HomeTeamShotsOnTarget,
                        AwayTeamShotsOnTarget = game.GameStatistic.AwayTeamShotsOnTarget,
                        HomeTeamShotsWide = game.GameStatistic.HomeTeamShotsWide,
                        AwayTeamShotsWide = game.GameStatistic.AwayTeamShotsWide,
                        HomeTeamFouls = game.GameStatistic.HomeTeamFouls,
                        AwayTeamFouls = game.GameStatistic.AwayTeamFouls,
                        HomeTeamOffsides = game.GameStatistic.HomeTeamOffsides,
                        AwayTeamOffsides = game.GameStatistic.AwayTeamOffsides
                    };

                    this.db.AddRange(gameStatistic);
                    this.db.SaveChanges();
                }

                // Add current game
                var existGame = new FootballGame
                {
                    MatchDate = game.MatchDate,
                    League = league,
                    HomeTeam = homeTeam,
                    AwayTeam = awayTeam,
                    FullTimeResult = fullTimeResult,
                    FirstHalfResult = firstHalf,
                    GameStatistic = gameStatistic
                };

                this.db.AddRange(existGame);
                this.db.SaveChanges();
                                

                // Add full time result to the DB and to current game
               

                this.db.SaveChanges();
            }
        }
    }
}
