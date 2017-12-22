namespace FootballAnalyzes.Services
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Prediction;

    public interface IPredictionService
    {
        IEnumerable<AllListingSM> All();
        PredictGamesSM PredictGames(string name);
    }
}
