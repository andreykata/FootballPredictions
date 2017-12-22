namespace FootballAnalyzes.Services
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Admin.Models;
    using FootballAnalyzes.Services.Models;

    public interface ILeagueService
    {
        IEnumerable<LeagueListingSM> All(int page = 1);
        int TotalLeaguesCount();
        string NameById(int leagueId);
        IEnumerable<FootballGamePM> LeagueGames(int leagueId);
    }
}
