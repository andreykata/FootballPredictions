using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballAnalyzes.Services;
using FootballAnalyzes.Web.Models;
using FootballAnalyzes.Web.Models.Games;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalyzes.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService games;

        public GamesController(IGameService games)
        {
            this.games = games;
        }

        public IActionResult All(int page = 1)
        {
            var allGames = new GameListingVM
            {
                Games = this.games.All(page),
                Page = new Pagination
                {
                    TotalCount = this.games.TotalGamesCount(),
                    CurrentPage = page
                }                
            };

            return View(allGames);
        }

        public IActionResult Next(int page = 1)
        {
            var nextGames = new GameListingVM
            {
                Games = this.games.Next(page),
                Page = new Pagination
                {
                    TotalCount = this.games.TotalNextGamesCount(),
                    CurrentPage = page
                }
            };

            return View(nextGames);
        }
    }
}