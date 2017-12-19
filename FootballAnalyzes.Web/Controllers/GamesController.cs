using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FootballAnalyzes.Services;
using FootballAnalyzes.Services.Models.Games;
using FootballAnalyzes.Web.Models;
using FootballAnalyzes.Web.Models.Games;
using Microsoft.AspNetCore.Authorization;
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
            page = page < 1 ? 1 : page;

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

        public IActionResult ByDate(int page = 1)
        {            
            var model = new ByDateListingVM
            {
                GamesByDate = this.games.GroupByDate(page),
                Page = new Pagination
                {
                    TotalCount = games.TotalByDateCount(),
                    CurrentPage = page
                }
            };

            return View(model);
        }

        public IActionResult WithoutResult(string date, int page = 1)
        {
            DateTime currentDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var nextGames = new GameListingVM
            {
                Games = this.games.WithoutResult(currentDate, page),
                Page = new Pagination
                {
                    TotalCount = this.games.TotalCurrnetDateGamesCount(currentDate),
                    CurrentPage = page
                }
            };

            return View(nextGames);
        }

        [Authorize(Roles = WebConstants.AdministratorRole)]
        public IActionResult EditGame(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var editGame = this.games.FindGameForEdit(id);

            return View(editGame);
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRole)]
        public IActionResult EditGame(EditGameSM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updateGameToDB = this.games.EditGame(model.Id, model.FullTimeResult.HomeTeamGoals, model.FullTimeResult.AwayTeamGoals);

            if (!updateGameToDB)
            {
                return NotFound();
            }

            string dateString = model.MatchDate.ToString("dd-MM-yyyy");

            return RedirectToAction(nameof(WithoutResult), new { date = dateString });
        }
    }
}