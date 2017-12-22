namespace FootballAnalyzes.Web.Models.Leagues
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Admin.Models;

    public class LeagueDetailsVM
    {
        public string Name { get; set; }
        public IEnumerable<FootballGamePM> Games { get; set; }
    }
}
