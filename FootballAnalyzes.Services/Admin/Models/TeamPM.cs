using System;
using System.Collections.Generic;
using System.Text;
using FootballAnalyzes.Services.Models.Games;

namespace FootballAnalyzes.Services.Admin.Models
{
    public class TeamPM : TeamSM
    {
        public IEnumerable<FootballGamePM> Games { get; set; }
    }
}
