namespace FootballAnalyzes.Web.Controllers
{
    using FootballAnalyzes.Services;

    using FootballAnalyzes.Web.Models;
    using FootballAnalyzes.Web.Models.Teams;
    using Microsoft.AspNetCore.Mvc;

    public class TeamsController : Controller
    {
        private readonly ITeamService teams;

        public TeamsController(ITeamService teams)
        {
            this.teams = teams;
        }

        public IActionResult All(int page = 1)
        {
            var allTeams = new TeamListingVM
            {
                Teams = this.teams.All(page),
                Page = new Pagination
                {
                    CurrentPage = page,
                    TotalCount = this.teams.TotalTeamsCount()
                }
            };

            return View(allTeams);
        }
    }
}