namespace FootballAnalyzes.Web.Models.Games
{
    using System;
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Games;

    using static FootballAnalyzes.Services.ServiceConstants;

    public class GameListingVM
    {
        public IEnumerable<FootballGameSM> Games { get; set; }
        public Pagination Page { get; set; }
    }
}
