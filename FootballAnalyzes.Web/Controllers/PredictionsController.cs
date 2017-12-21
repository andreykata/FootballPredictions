namespace FootballAnalyzes.Web.Controllers
{
    using FootballAnalyzes.Services;
    using Microsoft.AspNetCore.Mvc;

    public class PredictionsController : Controller
    {
        private readonly IPredictionService predictions;

        public PredictionsController(IPredictionService predictions)
        {
            this.predictions = predictions;
        }

        public IActionResult All()
        {
            var model = this.predictions.All();

            return View(model);
        }

        public IActionResult Predict(string name)
        {
            var model = this.predictions.PredictGames(name);

            return View(model);
        }
    }
}