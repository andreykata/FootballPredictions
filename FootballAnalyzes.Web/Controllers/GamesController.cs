using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballAnalyzes.Services;
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

        public IActionResult All()
        {
            var list = this.games.All();

            return View(list);
        }

        public IActionResult Update()
        {
            this.games.UpdateDb();

            return this.View();
        }
    }
}