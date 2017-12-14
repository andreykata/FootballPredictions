namespace FootballAnalyzes.Services
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models;

    public interface ILeagueService
    {
        IEnumerable<LeagueListingSM> All(int page = 1);
        int TotalLeaguesCount();
    }
}
