using System;
using System.Collections.Generic;
using System.Text;
using FootballAnalyzes.Services.Admin.Models;

namespace FootballAnalyzes.Services.Models.Prediction
{
    public class PredictGamesSM
    {
        public string Name { get; set; }
        public IEnumerable<FootballGamePM> Games { get; set; }
        public double WinProcent { get; set; }
    }
}
