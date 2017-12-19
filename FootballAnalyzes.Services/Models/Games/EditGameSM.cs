using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalyzes.Services.Models.Games
{
    public class EditGameSM
    {
        public int Id { get; set; }
        public DateTime MatchDate { get; set; }
        public string LeagueUniqueName { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public GameResultSM FullTimeResult { get; set; }
    }
}
