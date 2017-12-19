namespace FootballAnalyzes.Web.Areas.Admin.Controllers
{
    using System;
    using System.Globalization;
    using FootballAnalyzes.Services.Admin;
    using FootballAnalyzes.Web.Areas.Admin.Models.Update;
    using FootballAnalyzes.Web.Controllers;
    using FootballAnalyzes.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(WebConstants.AdminArea)]
    [Authorize(Roles = WebConstants.AdministratorRole)]
    public class UpdateController : Controller
    {
        private readonly IAdminUpdateService updates;

        public UpdateController(IAdminUpdateService updates)
        {
            this.updates = updates;
        }

        public IActionResult Index()
        {
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("bg-BG");
            string updateInfo = this.updates.UpdateDatesInfo();

            return View(new UpdateDetailsFM
            {
                DatesInfo = updateInfo,
                NextGamesDate = DateTime.Now
            });
        }

        [HttpPost]
        public IActionResult Index(UpdateDetailsFM model)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            string message = this.updates.UpdateDb(model.NextGamesDate);

            TempData.AddSuccessMessage($"{message}");

            return this.RedirectToAction(nameof(Index));
        }
                
        public IActionResult DeleteByDate(string date)
        {
            int deleteGamesCount = this.updates.DeleteGamesByDateCount(date);

            return View(new DeleteGamesByDateVM
            {
                Date = date,
                DeleteGamesCount = deleteGamesCount
            });
        }
                
        public IActionResult ConfirmDeleteByDate(string date, int count)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(GamesController.ByDate), "Games", new { area = "" });
            }

            this.updates.DeleteGamesByDate(date);
            TempData.AddSuccessMessage($"Successfull deleted {count} games.");

            return RedirectToAction(nameof(GamesController.ByDate), "Games", new { area = "" });
        }
        
        public IActionResult UpdateOldGames()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateOldGames(UpdateOldGamesForResultFM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var message = this.updates.UpdateOldGames(model.StartDate, model.EndDate);

            TempData.AddSuccessMessage($"{message}");

            return RedirectToAction(nameof(GamesController.ByDate), "Games", new { area = "" });
        }

        public IActionResult UpdateCurrentDateGames(string date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            DateTime currentDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var message = this.updates.UpdateOldGames(currentDate, currentDate);

            TempData.AddSuccessMessage($"{message}");

            return RedirectToAction(nameof(GamesController.ByDate), "Games", new { area = "" });
        }

        public IActionResult Predict()
        {
            this.updates.MakePredictionToOldGames();

            return View();
        }
    }
}