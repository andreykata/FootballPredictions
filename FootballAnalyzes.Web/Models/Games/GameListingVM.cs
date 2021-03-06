﻿namespace FootballAnalyzes.Web.Models.Games
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Admin.Models;
    using FootballAnalyzes.Services.Models.Games;

    public class GameListingVM
    {
        public IEnumerable<FootballGamePM> Games { get; set; }
        public Pagination Page { get; set; }
    }
}
