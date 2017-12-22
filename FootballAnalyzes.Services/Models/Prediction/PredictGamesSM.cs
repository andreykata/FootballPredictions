namespace FootballAnalyzes.Services.Models.Prediction
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Admin.Models;

    public class PredictGamesSM
    {
        public string Name { get; set; }
        public IEnumerable<FootballGamePM> Games { get; set; }
        public double WinProcent { get; set; }
    }
}
