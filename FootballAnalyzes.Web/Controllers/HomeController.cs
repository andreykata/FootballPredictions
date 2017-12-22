namespace FootballAnalyzes.Web.Controllers
{
    using FootballAnalyzes.Services;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IHomeService homes;

        public HomeController(IHomeService homes)
        {
            this.homes = homes;
        }

        public IActionResult Index()
        {
            var model = this.homes.HomeInfo();

            return View(model);
        }
    }
}
