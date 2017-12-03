namespace FootballAnalyzes.UpdateDatabase
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using FootballAnalyzes.Data.Models;
    using FootballAnalyzes.UpdateDatabase.BindingModels;
    using FootballAnalyzes.Web.Data;

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

                TeamBM homeTeamBM = new TeamBM()
                {
                    Name = gameParams[7],
                    UniqueName = gameParams[6]
                };

                TeamBM awayTeamBM = new TeamBM()
                {
                    Name = gameParams[9],
                    UniqueName = gameParams[8]
                };

                LeagueBM leagueBM = new LeagueBM()
                {
                    Country = gameParams[2],
                    Name = gameParams[3].Substring(0, gameParams[3].LastIndexOf('-')),
                    Year = gameParams[3].Substring(gameParams[3].LastIndexOf('-') + 1),
                    Stage = gameParams[4],
                    Type = gameParams[5]
                };
                GameStatisticBM statBM = new GameStatisticBM()
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


                // Add home team to DB
                var homeTeam = db
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

                // Add away team to DB
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

                // Add league to DB
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

                

                //FootballGame currentGame = new FootballGame()
                //{
                //    Date = DateTime.ParseExact(gameParams[0], "yyyyMMdd", CultureInfo.InvariantCulture),
                //    KickOff = gameParams[1],
                //    League = league,
                //    HomeTeam = homeTeam,
                //    AwayTeam = awayTeam,
                //    FullTimeHomeTeamGoals = int.Parse(gameParams[11]),
                //    FullTimeAwayTeamGoals = int.Parse(gameParams[12]),
                //    HalfTimeHomeTeamGoals = int.Parse(gameParams[14]),
                //    HalfTimeAwayTeamGoals = int.Parse(gameParams[15]),
                //    GameStatistics = stat,
                //};


                //this.games.Add(currentGame);
                //this.teams[homeTeam.UniqueName].FootballGames.Add(currentGame);
                //this.teams[awayTeam.UniqueName].FootballGames.Add(currentGame);
                //this.leagues[league.UniqueName].FootballGames.Add(currentGame);


                
            }
        }
    }
}
