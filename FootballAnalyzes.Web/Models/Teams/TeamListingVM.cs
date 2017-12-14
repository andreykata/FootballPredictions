namespace FootballAnalyzes.Web.Models.Teams
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Teams;

    public class TeamListingVM
    {
        public IEnumerable<TeamListingSM> Teams { get; set; }

        public Pagination Page { get; set; }
    }
}
