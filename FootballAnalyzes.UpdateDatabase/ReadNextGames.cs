namespace FootballAnalyzes.UpdateDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using FootballAnalyzes.UpdateDatabase.BindingModels;

    public class ReadNextGames
    {
        public static int counter = 0;
        public static List<FootballGameBM> allGames = new List<FootballGameBM>();
        public static List<string> errors = new List<string>();

        public List<FootballGameBM> ReadDataFromWeb(DateTime currentDate)
        {
            Console.WriteLine("Reading games from www.soccerway.com ...");

            try
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

                var pattern = "<td\\sclass=\\\\\"score-time\\sstatus\\\\\"><a\\shref=\\\\\"(?'GameLink'[^?]+)";
                Regex rgx = new Regex(pattern);
                MatchCollection matches = rgx.Matches(text);

                foreach (Match matchGame in matches)
                {
                    string gameLink = matchGame.Groups["GameLink"].Value.Trim().Replace("\\", "");

                    ReadGameInfo(gameLink, currentDate);
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
            var pattern = "<td\\sclass=\"score-time\\sstatus\">\\s*<a\\shref=\"(?'GameLink'[^?]+)";
            Regex rgx = new Regex(pattern);
            MatchCollection matches = rgx.Matches(text);

            foreach (Match match in matches)
            {
                string gameLink = match.Groups["GameLink"].Value.Trim();

                ReadGameInfo(gameLink, currentDate);
            }
        }

        private static void ReadGameInfo(string gameLink, DateTime currentDate)
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


                // Create football game
                FootballGameBM game = new FootballGameBM
                {
                    League = league,
                    MatchDate = currentDate,
                    HomeTeam = homeTeam,
                    AwayTeam = awayTeam
                };
                
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
    }
}
