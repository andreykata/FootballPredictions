namespace FootballAnalyzes.UpdateDatabase
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Data.Models;
    using FootballAnalyzes.UpdateDatabase.BindingModels;

    public static class Seed
    {
        public static async Task ReadGamesFromFile(FootballAnalyzesDbContext db)
        {
            string input;

            using (StreamReader reader = new StreamReader("../FootballAnalyzes.UpdateDatabase/all_games.txt"))
            {
                input = reader.ReadToEnd();
            }

            string[] allGamesFromFile = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var game in allGamesFromFile)
            {
                string[] gameParams = game.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Team homeTeam = await AddHomeTeam(db, gameParams);
                Team awayTeam = await AddAwayTeam(db, gameParams);
                League league = await AddLeague(db, gameParams);
                FootballGame currentGame = await AddGame(db, gameParams, homeTeam, awayTeam, league);
                await AdduFullTimeResult(db, gameParams, currentGame);
                await AddFirstHalfResult(db, gameParams, currentGame);
                await AddGameStatistic(db, gameParams, currentGame);

                homeTeam.Games.Add(currentGame);
                awayTeam.Games.Add(currentGame);
                league.Games.Add(currentGame);

                await db.SaveChangesAsync();
            }
        }

        private static async Task AddGameStatistic(FootballAnalyzesDbContext db, string[] gameParams, FootballGame currentGame)
        {
            // Add game statistic to DB and to the game IF EXCIST
            if (gameParams[16] != "-1" && gameParams[17] != "-1")
            {
                var gameStatistic = new GameStatistic()
                {
                    HomeTeamCorners = int.Parse(gameParams[16]),
                    AwayTeamCorners = int.Parse(gameParams[17]),
                    HomeTeamShotsOnTarget = int.Parse(gameParams[18]),
                    AwayTeamShotsOnTarget = int.Parse(gameParams[19]),
                    HomeTeamShotsWide = int.Parse(gameParams[20]),
                    AwayTeamShotsWide = int.Parse(gameParams[21]),
                    HomeTeamFouls = int.Parse(gameParams[22]),
                    AwayTeamFouls = int.Parse(gameParams[23]),
                    HomeTeamOffsides = int.Parse(gameParams[24]),
                    AwayTeamOffsides = int.Parse(gameParams[25]),
                    GameId = currentGame.Id,
                    Game = currentGame,
                };

                db.Add(gameStatistic);
                await db.SaveChangesAsync();

                currentGame.GameStatisticId = gameStatistic.Id;
                currentGame.GameStatistic = gameStatistic;
            }
        }

        private static async Task AddFirstHalfResult(FootballAnalyzesDbContext db, string[] gameParams, FootballGame currentGame)
        {
            // Add first half result to DB and to the game IF EXCIST    
            if (gameParams[14] != "-1" && gameParams[15] != "-1")
            {
                GameResult halfTimeResult = new GameResult()
                {
                    Result = (ResultEnum)Enum.Parse(typeof(ResultEnum), gameParams[13], true),
                    HomeTeamGoals = int.Parse(gameParams[14]),
                    AwayTeamGoals = int.Parse(gameParams[15]),
                    GameId = currentGame.Id,
                    Game = currentGame
                };

                db.Add(halfTimeResult);
                await db.SaveChangesAsync();

                currentGame.FirstHalfResultId = halfTimeResult.Id;
                currentGame.FirstHalfResult = halfTimeResult;
            }
        }

        private static async Task AdduFullTimeResult(FootballAnalyzesDbContext db, string[] gameParams, FootballGame currentGame)
        {
            // Add full time result to DB and to the game
            var fullTimeResult = new GameResult()
            {
                Result = (ResultEnum)Enum.Parse(typeof(ResultEnum), gameParams[10], true),
                HomeTeamGoals = int.Parse(gameParams[11]),
                AwayTeamGoals = int.Parse(gameParams[12]),
                GameId = currentGame.Id,
                Game = currentGame
            };

            db.Add(fullTimeResult);
            await db.SaveChangesAsync();

            currentGame.FullTimeResultId = fullTimeResult.Id;
            currentGame.FullTimeResult = fullTimeResult;
        }

        private static async Task<FootballGame> AddGame(FootballAnalyzesDbContext db, string[] gameParams, Team homeTeam, Team awayTeam, League league)
        {
            string time = gameParams[1] == "33:33" ? "00:59" : gameParams[1];
            var matchDate = DateTime.ParseExact(gameParams[0] + time, "yyyyMMddHH:mm", CultureInfo.InvariantCulture);

            // Add game to DB
            FootballGame currentGame = new FootballGame()
            {
                MatchDate = matchDate,
                League = league,
                LeagueId = league.Id,
                HomeTeam = homeTeam,
                HomeTeamId = homeTeam.Id,
                AwayTeam = awayTeam,
                AwayTeamId = awayTeam.Id
            };

            db.Add(currentGame);
            await db.SaveChangesAsync();
            return currentGame;
        }

        private static async Task<League> AddLeague(FootballAnalyzesDbContext db, string[] gameParams)
        {
            // Add current league in DB if it doesn't exist
            LeagueBM leagueBM = new LeagueBM()
            {
                Country = gameParams[2],
                Name = gameParams[3].Substring(0, gameParams[3].LastIndexOf('-')),
                Year = gameParams[3].Substring(gameParams[3].LastIndexOf('-') + 1),
                Stage = gameParams[4],
                Type = gameParams[5]
            };

            var league = db
                .Leagues
                .Where(l => l.UniqueName.Equals(leagueBM.UniqueName))
                .FirstOrDefault();

            if (league == null)
            {
                league = new League()
                {
                    Country = leagueBM.Country,
                    Name = leagueBM.Name,
                    Year = leagueBM.Year,
                    Stage = leagueBM.Stage,
                    Type = leagueBM.Type,
                    UniqueName = $"{leagueBM.Country},{leagueBM.Name},{leagueBM.Year},{leagueBM.Stage},{leagueBM.Type}"
                };

                await db.AddAsync(league);
                await db.SaveChangesAsync();
            }

            return league;
        }

        private static async Task<Team> AddAwayTeam(FootballAnalyzesDbContext db, string[] gameParams)
        {
            // Add away team in DB if it doesn't exist
            TeamBM awayTeamBM = new TeamBM()
            {
                Name = gameParams[9],
                UniqueName = gameParams[8]
            };

            var awayTeam = db
                .Teams
                .Where(t => t.UniqueName == awayTeamBM.UniqueName)
                .FirstOrDefault();

            if (awayTeam == null)
            {
                awayTeam = new Team();
                awayTeam.Name = awayTeamBM.Name;
                awayTeam.UniqueName = awayTeamBM.UniqueName;

                await db.Teams.AddAsync(awayTeam);
                await db.SaveChangesAsync();
            }

            return awayTeam;
        }

        private static async Task<Team> AddHomeTeam(FootballAnalyzesDbContext db, string[] gameParams)
        {
            // Add home team in DB if it doesn't exist
            TeamBM homeTeamBM = new TeamBM()
            {
                Name = gameParams[7],
                UniqueName = gameParams[6]
            };

            Team homeTeam = db
                .Teams
                .Where(t => t.UniqueName == homeTeamBM.UniqueName)
                .FirstOrDefault();

            if (homeTeam == null)
            {
                homeTeam = new Team();
                homeTeam.Name = homeTeamBM.Name;
                homeTeam.UniqueName = homeTeamBM.UniqueName;

                await db.Teams.AddAsync(homeTeam);
                await db.SaveChangesAsync();
            }

            return homeTeam;
        }
    }
}
