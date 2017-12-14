namespace FootballAnalyzes.UpdateDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using FootballAnalyzes.Data.Models;
    using FootballAnalyzes.UpdateDatabase.BindingModels;

    public class ReadDataWeb
    {
        public static int counter = 0;
        public static List<FootballGameBM> allGames = new List<FootballGameBM>();
        public static List<string> errors = new List<string>();

        public List<FootballGameBM> ReadDataFromWeb(DateTime currentDate)
        {
            Console.WriteLine("Reading games from www.soccerway.com ...");

            try
            {
                currentDate = currentDate.AddDays(1);

                while (currentDate.Date < DateTime.Now.AddDays(-1).Date)
                {

                    string currentStringDate = currentDate.ToString("yyyy//MM//dd");
                    string requestUri = $"http://int.soccerway.com/matches/{currentDate.Year}/{currentDate.Month:00}/{currentDate.Day:00}/";

                    var request = WebRequest.Create(requestUri);
                    request.ContentType = "application/json; charset=utf-8";
                    string text = "";

                    Stopwatch responseTime = new Stopwatch();
                    responseTime.Start();
                    var response = (HttpWebResponse)request.GetResponse();
                    responseTime.Stop();

                    Console.WriteLine($"{currentStringDate} - {responseTime.Elapsed}");

                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        text = sr.ReadToEnd();
                    }

                    // Read Expanded leagues and their games
                    ReadExpandedLeagues(text, currentDate);

                    // Read Clickable leagues and their games
                    ReadClickableLeagues(text, currentDate);

                    currentDate = currentDate.AddDays(1);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                errors.Add($"{currentDate}\n{e}");
            }

            return allGames;
        }
        
        private static void ReadClickableLeagues(string text, DateTime currentDate)
        {
            var pattern = "<tr\\sclass=\"group-head\\s+clickable\\s\"\\s+id=\"date_matches-(?'GameId'[\\d]+)\"\\s+stage-value=\"(?'StageValue'[\\d]+)";

            Regex rgx = new Regex(pattern);
            MatchCollection matches = rgx.Matches(text);

            foreach (Match match in matches)
            {
                ReadClickableLeagueGames(currentDate, match);
            }
        }

        private static void ReadClickableLeagueGames(DateTime currentDate, Match match)
        {
            string gameId = match.Groups["GameId"].Value.Trim();
            string stageValue = match.Groups["StageValue"].Value.Trim();

            try
            {
                string requestUri = $"http://int.soccerway.com/a/block_date_matches?block_id=page_matches_1_block_date_matches_1&callback_params={{\"bookmaker_urls\":{{\"13\":[{{\"link\":\"http://www.bet365.com/home/?affiliate=365_179024\",\"name\":\"Bet 365\"}}]}},\"block_service_id\":\"matches_index_block_datematches\",\"date\":\"{currentDate.Year}-{currentDate.Month}-{currentDate.Day}\",\"stage-value\":\"{stageValue}\"}}&action=showMatches&params={{\"competition_id\":{gameId}}}";

                var request = WebRequest.Create(requestUri);
                request.ContentType = "application/json; charset=utf-8";
                string text = "";

                Stopwatch responseTime = new Stopwatch();
                responseTime.Start();
                var response = (HttpWebResponse)request.GetResponse();
                responseTime.Stop();

                Console.WriteLine($"{stageValue}, {gameId} - {responseTime.Elapsed}");

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd();
                }

                var pattern = "<td\\sclass=\\\\\"score-time\\sscore\\\\\"><a\\shref=\\\\\"(?'GameLink'[^?]+)";
                Regex rgx = new Regex(pattern);
                MatchCollection matches = rgx.Matches(text);

                foreach (Match matchGame in matches)
                {
                    string gameLink = matchGame.Groups["GameLink"].Value.Trim().Replace("\\", "");

                    ReadGameStatistic(gameLink, currentDate);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                errors.Add($"Game ID: {gameId}, Stage value: {stageValue}\n{e}");
            }
        }

        private static void ReadExpandedLeagues(string text, DateTime currentDate)
        {
            // Read open leagues (expanded loaded)
            var pattern = "<td\\sclass=\"score-time\\sscore\">\\s*<a\\shref=\"(?'GameLink'[^?]+)";
            Regex rgx = new Regex(pattern);
            MatchCollection matches = rgx.Matches(text);

            foreach (Match match in matches)
            {
                string gameLink = match.Groups["GameLink"].Value.Trim();

                ReadGameStatistic(gameLink, currentDate);
            }
        }

        private static void ReadGameStatistic(string gameLink, DateTime currentDate)
        {
            try
            {
                string requestUri = $"http://int.soccerway.com{gameLink}/";

                var request = WebRequest.Create(requestUri);
                request.ContentType = "application/json; charset=utf-8";
                string text = "";

                Stopwatch responseTime = new Stopwatch();
                responseTime.Start();
                var response = (HttpWebResponse)request.GetResponse();
                responseTime.Stop();

                Console.WriteLine($"\t{counter}: {gameLink} - {responseTime.Elapsed}");
                counter++;

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd();
                }


                // Parse both team
                var pattern = "class=\"thick\">\\s+<a\\shref=\"(?'TeamLink'[^\"]+)\">(?'TeamName'[^<]+)";

                Regex rgx = new Regex(pattern);
                MatchCollection matches = rgx.Matches(text);

                string homeTeamName = matches[0].Groups["TeamName"].Value.Trim();
                string homeTeamUniqueName = matches[0].Groups["TeamLink"].Value.Trim().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[2];
                TeamBM homeTeam = new TeamBM
                {
                    UniqueName = homeTeamUniqueName,
                    Name = homeTeamName
                };

                string awayTeamName = matches[1].Groups["TeamName"].Value.Trim();
                string awayTeamUniqueName = matches[1].Groups["TeamLink"].Value.Trim().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[2];
                TeamBM awayTeam = new TeamBM
                {
                    UniqueName = awayTeamUniqueName,
                    Name = awayTeamName
                };

                // Parse league
                pattern = "<dt>Competition<\\/dt>\\s+<dd><a\\shref=\"(?'LeagueLink'[^\"]+)";
                rgx = new Regex(pattern);
                Match leagueMatch = rgx.Match(text);
                string[] leagueData = leagueMatch.Groups["LeagueLink"].Value.Trim().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string leagueType = leagueData[0];
                string leagueCountry = leagueData[1];
                string leagueName = leagueData[2];
                string leagueYear = leagueData[3];
                string leagueStage = leagueData[4];

                LeagueBM league = new LeagueBM
                {
                    Type = leagueType,
                    Country = leagueCountry,
                    Name = leagueName,
                    Year = leagueYear,
                    Stage = leagueStage
                };

                // Pars kick-off
                pattern = "<dt>Kick-off<\\/dt>\\s+<dd>\\s+<[^>]+>(?'KickOff'[^<]+)";
                rgx = new Regex(pattern);
                Match kickOffMatch = rgx.Match(text);
                string kickOff = kickOffMatch.Groups["KickOff"].Value.Trim() != "" ? kickOffMatch.Groups["KickOff"].Value.Trim() : "00:59"; 
                currentDate = currentDate.AddHours(int.Parse(kickOff.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0]));
                currentDate = currentDate.AddMinutes(int.Parse(kickOff.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]));

                // Parse full time goals
                pattern = "<dt>Full-time<\\/dt>\\s+<dd>(?'HomeTeamFullTimeGoals'\\d+)\\s+-\\s*(?'AwayTeamFullTimeGoals'\\d+)";
                rgx = new Regex(pattern);
                Match goalsMatch = rgx.Match(text);
                int homeTeamFullTimeGoals = int.Parse(goalsMatch.Groups["HomeTeamFullTimeGoals"].Value.Trim());
                int awayTeamFullTimeGoals = int.Parse(goalsMatch.Groups["AwayTeamFullTimeGoals"].Value.Trim());
                ResultEnum fullTimeResult = homeTeamFullTimeGoals > awayTeamFullTimeGoals ? ResultEnum.H : (homeTeamFullTimeGoals < awayTeamFullTimeGoals ? ResultEnum.A : ResultEnum.D);
                GameResultBM fullTimeGameResult = new GameResultBM
                {
                    Result = fullTimeResult,
                    HomeTeamGoals = homeTeamFullTimeGoals,
                    AwayTeamGoals = awayTeamFullTimeGoals
                };

                // It's not sure we have half-time stats
                pattern = "<dt>Half-time<\\/dt>\\s+<dd>(?'HomeTeamHalfTimeGoals'\\d+)\\s+-\\s*(?'AwayTeamHalfTimeGoals'\\d+)<\\/dd>\\s+";
                rgx = new Regex(pattern);
                goalsMatch = rgx.Match(text);
                GameResultBM halfTimeGameResult = null;

                if (goalsMatch.Groups.Count == 3)
                {
                    int homeTeamHalfTimeGoals = int.Parse(goalsMatch.Groups["HomeTeamHalfTimeGoals"].Value.Trim());
                    int awayTeamHalfTimeGoals = int.Parse(goalsMatch.Groups["AwayTeamHalfTimeGoals"].Value.Trim());
                    ResultEnum halfTimeResult = homeTeamHalfTimeGoals > awayTeamHalfTimeGoals ? ResultEnum.H : (homeTeamHalfTimeGoals < awayTeamHalfTimeGoals ? ResultEnum.A : ResultEnum.D);

                    halfTimeGameResult = new GameResultBM
                    {
                        Result = halfTimeResult,
                        HomeTeamGoals = homeTeamHalfTimeGoals,
                        AwayTeamGoals = awayTeamFullTimeGoals
                    };
                }

                // Create football game
                FootballGameBM game = new FootballGameBM
                {
                    League = league,
                    MatchDate = currentDate,
                    HomeTeam = homeTeam,
                    AwayTeam = awayTeam,
                    FullTimeResult = fullTimeGameResult,
                    FirstHalfResult = halfTimeGameResult
                };

                // Parse game statistics
                GameStatisticBM stats = ExtractGameStatistics(text);
                game.GameStatistic = stats;

                Console.WriteLine($"\t\t{game}");

                // Add game in collection
                allGames.Add(game);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                errors.Add($"{gameLink}\n{e}");
            }

        }

        private static GameStatisticBM ExtractGameStatistics(string text)
        {
            string idLink = "";
            try
            {
                var pattern = "charts\\/statsplus\\/(?'Id'\\d+)";
                Regex rgx = new Regex(pattern);
                Match statsMatch = rgx.Match(text);

                if (statsMatch.Groups.Count != 2)
                {
                    return null;
                }

                idLink = statsMatch.Groups["Id"].Value.Trim();

                string requestUri = $"http://int.soccerway.com/charts/statsplus/{idLink}/";

                var request = WebRequest.Create(requestUri);
                request.ContentType = "application/json; charset=utf-8";
                string statsText = "";

                Stopwatch responseTime = new Stopwatch();
                responseTime.Start();
                var response = (HttpWebResponse)request.GetResponse();
                responseTime.Stop();

                Console.WriteLine($"\t\t{idLink} - {responseTime.Elapsed}");

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    statsText = sr.ReadToEnd();
                }

                pattern = "legend\\sleft\\svalue'>(?'HomeTeamCorners'\\d+)<\\/td>\\s+.*\\s+[^>]+>(?'AwayTeamCorners'\\d+).*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+[^>]+>(?'HomeTeamShotsOnTarget'\\d+).*\\s+.*\\s+[^>]+>(?'AwayTeamShotsOnTarget'\\d+).*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+[^>]+>(?'HomeTeamShotsWide'\\d+).*\\s+.*\\s+[^>]+>(?'AwayTeamShotsWide'\\d+).*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+[^>]+>(?'HomeTeamFouls'\\d+).*\\s+.*\\s+[^>]+>(?'AwayTeamFouls'\\d+).*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+.*\\s+[^>]+>(?'HomeTeamOffsides'\\d+).*\\s+.*\\s+[^>]+>(?'AwayTeamOffsides'\\d+)";
                rgx = new Regex(pattern);
                Match statsParamsMatch = rgx.Match(statsText);

                if (statsParamsMatch.Groups.Count < 11)
                {
                    return new GameStatisticBM();
                }

                GameStatisticBM stats = new GameStatisticBM
                {
                    HomeTeamCorners = int.Parse(statsParamsMatch.Groups["HomeTeamCorners"].Value.Trim()),
                    AwayTeamCorners = int.Parse(statsParamsMatch.Groups["AwayTeamCorners"].Value.Trim()),
                    HomeTeamShotsOnTarget = int.Parse(statsParamsMatch.Groups["HomeTeamShotsOnTarget"].Value.Trim()),
                    AwayTeamShotsOnTarget = int.Parse(statsParamsMatch.Groups["AwayTeamShotsOnTarget"].Value.Trim()),
                    HomeTeamShotsWide = int.Parse(statsParamsMatch.Groups["HomeTeamShotsWide"].Value.Trim()),
                    AwayTeamShotsWide = int.Parse(statsParamsMatch.Groups["AwayTeamShotsWide"].Value.Trim()),
                    HomeTeamFouls = int.Parse(statsParamsMatch.Groups["HomeTeamFouls"].Value.Trim()),
                    AwayTeamFouls = int.Parse(statsParamsMatch.Groups["AwayTeamFouls"].Value.Trim()),
                    HomeTeamOffsides = int.Parse(statsParamsMatch.Groups["HomeTeamOffsides"].Value.Trim()),
                    AwayTeamOffsides = int.Parse(statsParamsMatch.Groups["AwayTeamOffsides"].Value.Trim())
                };

                return stats;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                errors.Add($"{idLink}\n{e}");
                return new GameStatisticBM();
            }

        }
    }
}
