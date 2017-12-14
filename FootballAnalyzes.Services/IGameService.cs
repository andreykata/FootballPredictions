namespace FootballAnalyzes.Services
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Games;

    public interface IGameService
    {
        IEnumerable<FootballGameSM> All(int page = 1);
        IEnumerable<FootballGameSM> Next(int page = 1);
        int TotalGamesCount();
        int TotalNextGamesCount();
    }
}
