namespace FootballAnalyzes.Web.Areas.Admin.Controllers
{
    using FootballAnalyzes.Services.Admin;
    using FootballAnalyzes.Web.Areas.Admin.Models.Update;
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
            return View();
        }

        [HttpPost]
        public IActionResult Index(UpdateDetailsFM model)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }
            this.updates.UpdateDb(model.NextGamesDate);

            return this.RedirectToAction(nameof(Index));
        }
    }
}