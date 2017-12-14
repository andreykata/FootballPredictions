namespace FootballAnalyzes.Web.Models.Leagues
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models;

    public class LeagueListingVM
    {
        public IEnumerable<LeagueListingSM> Leagues { get; set; }
        public Pagination Page { get; set; }
    }
}
