namespace FootballAnalyzes.Services.Admin.Models
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Games;

    public class FootballGamePM : FootballGameSM
    {
        public List<PredictionSM> Predictions { get; set; } = new List<PredictionSM>();

        public override string ToString()
        {
            string result = base.ToString();

            if (this.Predictions.Count != 0)
            {
                foreach (var predict in this.Predictions)
                {
                    result += $"{predict.Name}: {predict.Procent}";
                }
            }

            return result;
        }
    }
}
