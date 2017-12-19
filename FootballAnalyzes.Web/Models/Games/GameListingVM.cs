namespace FootballAnalyzes.Web.Models.Games
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Games;

    public class GameListingVM
    {
        public IEnumerable<FootballGameSM> Games { get; set; }
        public Pagination Page { get; set; }
    }
}
