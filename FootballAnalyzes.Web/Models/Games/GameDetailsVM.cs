namespace FootballAnalyzes.Web.Models.Games
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Admin.Models;

    public class GameDetailsVM
    {
        public FootballGamePM CurrentGame { get; set; }
        public IEnumerable<FootballGamePM> HomeTeamGames { get; set; }
        public IEnumerable<FootballGamePM> AwayTeamGames { get; set; }
        public IEnumerable<FootballGamePM> GamesBeteenBothTeams { get; set; }

    }
}
