namespace FootballAnalyzes.Web.Models.Teams
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Admin.Models;

    public class TeamDetailsVM
    {
        public string Name { get; set; }
        public IEnumerable<FootballGamePM> Games { get; set; }
    }
}
