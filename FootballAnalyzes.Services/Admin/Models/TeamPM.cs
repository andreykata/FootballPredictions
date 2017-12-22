namespace FootballAnalyzes.Services.Admin.Models
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Games;

    public class TeamPM : TeamSM
    {
        public IEnumerable<FootballGamePM> Games { get; set; }
    }
}
