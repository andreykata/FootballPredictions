using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballAnalyzes.Services;
using FootballAnalyzes.Web.Models;
using FootballAnalyzes.Web.Models.Leagues;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalyzes.Web.Controllers
{
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
    }
}