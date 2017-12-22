namespace FootballAnalyzes.Services.Admin.Models
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Games;

    public class LeaguePM : LeagueSM
    {
        public IEnumerable<FootballGamePM> Games { get; set; }
    }
}
