namespace FootballAnalyzes.Services
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Teams;

    public interface ITeamService
    {
        IEnumerable<TeamListingSM> All(int page = 1);
        int TotalTeamsCount();
    }
}
