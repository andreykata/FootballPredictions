namespace FootballAnalyzes.Web.Models.Games
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Games;

    public class ByDateListingVM
    {
        public IEnumerable<ByDateSM> GamesByDate { get; set; }
        public Pagination Page { get; set; }
    }
}
