namespace FootballAnalyzes.Web.Controllers
{
    using FootballAnalyzes.Services;
    using FootballAnalyzes.Web.Models;
    using FootballAnalyzes.Web.Models.Leagues;
    using Microsoft.AspNetCore.Mvc;


    public class LeaguesController : Controller
    {
        private readonly ILeagueService leagues;

        public LeaguesController(ILeagueService leagues)
        {
            this.leagues = leagues;
        }

        public IActionResult All(int page = 1)
        {
            var allLeagues = new LeagueListingVM
            {
                Leagues = this.leagues.All(page),
                Page = new Pagination
                {
                    TotalCount = this.leagues.TotalLeaguesCount(),
                    CurrentPage = page
                }
            };

            return View(allLeagues);
        }

        public IActionResult ById(int leagueId)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var name = this.leagues.NameById(leagueId);
            var games = this.leagues.LeagueGames(leagueId);

            if (name == null)
            {
                return NotFound();
            }

            return View(new LeagueDetailsVM
            {
                Name = name,
                Games = games
            }); ;
        }
    }
}