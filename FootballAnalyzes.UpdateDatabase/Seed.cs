namespace FootballAnalyzes.UpdateDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Data.Models;
    using FootballAnalyzes.UpdateDatabase.BindingModels;

    public static class Seed
    {

        public static List<FootballGameBM> ReadGamesFromFile()
        {
            List<FootballGameBM> allGames = new List<FootballGameBM>();
            string input;

            using (StreamReader reader = new StreamReader("../FootballAnalyzes.UpdateDatabase/all_games.txt"))
            {
                input = reader.ReadToEnd();
            }

            string[] allGamesFromFile = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var game in allGamesFromFile)
            {
                string[] gameParams = game.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                TeamBM homeTeamBM = new TeamBM()
                {
                    Name = gameParams[6],
                    UniqueName = gameParams[7]
                };

                // Add away team in DB if it doesn't exist
                TeamBM awayTeamBM = new TeamBM()
                {
                    Name = gameParams[8],
                    UniqueName = gameParams[9]
                };

                // Add current league in DB if it doesn't exist
                LeagueBM leagueBM = new LeagueBM()
                {
                    Country = gameParams[2],
                    Name = gameParams[3].Substring(0, gameParams[3].LastIndexOf('-')),
                    Year = gameParams[3].Substring(gameParams[3].LastIndexOf('-') + 1),
                    Stage = gameParams[4],
                    Type = gameParams[5]
                };

                string time = gameParams[1] == "33:33" ? "00:59" : gameParams[1];
                var matchDate = DateTime.ParseExact(gameParams[0] + time, "yyyyMMddHH:mm", CultureInfo.InvariantCulture);

                
                // Add full time result to DB and to the game
                var fullTimeResult = new GameResultBM()
                {
                    Result = (ResultEnum)Enum.Parse(typeof(ResultEnum), gameParams[10], true),
                    HomeTeamGoals = int.Parse(gameParams[11]),
                    AwayTeamGoals = int.Parse(gameParams[12])
                };

                GameResultBM halfTimeResult = null;
                // Add first half result to DB and to the game IF EXCIST    
                if (gameParams[14] != "-1" && gameParams[15] != "-1")
                {
                    halfTimeResult = new GameResultBM
                    {
                        Result = (ResultEnum)Enum.Parse(typeof(ResultEnum), gameParams[13], true),
                        HomeTeamGoals = int.Parse(gameParams[14]),
                        AwayTeamGoals = int.Parse(gameParams[15])
                    };
                }

                GameStatisticBM gameStatistic = null;
                if (gameParams[16] != "-1" && gameParams[17] != "-1")
                {
                    gameStatistic = new GameStatisticBM()
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
                        AwayTeamOffsides = int.Parse(gameParams[25])
                    };
                    
                }

                // Add game to DB
                FootballGameBM currentGame = new FootballGameBM
                {
                    MatchDate = matchDate,
                    League = leagueBM,
                    HomeTeam = homeTeamBM,
                    AwayTeam = awayTeamBM,
                    FullTimeResult = fullTimeResult,
                    FirstHalfResult = halfTimeResult,
                    GameStatistic = gameStatistic
                };

                allGames.Add(currentGame);
            }

            return allGames;
        }
    }
}
