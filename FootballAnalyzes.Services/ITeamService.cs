namespace FootballAnalyzes.Services
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Admin.Models;
    using FootballAnalyzes.Services.Models.Teams;

    public interface ITeamService
    {
        IEnumerable<TeamListingSM> All(int page = 1);
        int TotalTeamsCount();
        IEnumerable<FootballGamePM> TeamGames(int teamId);
        string NameById(int teamId);
    }
}
