namespace FootballAnalyzes.Web.Controllers
{
    using System;
    using System.Globalization;
    using FootballAnalyzes.Services;
    using FootballAnalyzes.Services.Models.Games;
    using FootballAnalyzes.Web.Models;
    using FootballAnalyzes.Web.Models.Games;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


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
            page = page < 1 ? 1 : page;

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
            page = page < 1 ? 1 : page;

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

        public IActionResult DateGames(string date, int page = 1)
        {
            page = page < 1 ? 1 : page;
            DateTime currentDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var games = this.games.DateGames(currentDate, page);

            return View(games);
        }

        public IActionResult WithoutResult(string date, int page = 1)
        {
            page = page < 1 ? 1 : page;

            DateTime currentDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var nextGames = new GameListingWithoutResultVM
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

        public IActionResult GameDetails(int gameId, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var currentGame = this.games.ById(gameId);

            if (currentGame == null)
            {
                return NotFound();
            }

            var hTGames = this.games.TeamGames(currentGame.MatchDate, currentGame.HomeTeam.Id, page);
            var aTGames = this.games.TeamGames(currentGame.MatchDate, currentGame.AwayTeam.Id, page);
            var gamesBetweenBothTeams = this.games.BetweenBothTeams(currentGame.MatchDate, currentGame.HomeTeam.Id, currentGame.AwayTeam.Id);

            var model = new GameDetailsVM
            {
                CurrentGame = currentGame,
                HomeTeamGames = hTGames,
                AwayTeamGames = aTGames,
                GamesBeteenBothTeams = gamesBetweenBothTeams
            };

            return View(model);
        }
    }
}