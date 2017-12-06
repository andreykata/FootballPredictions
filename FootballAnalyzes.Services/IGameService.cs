namespace FootballAnalyzes.Services
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Games;

    public interface IGameService
    {
        IEnumerable<FootballGameSM> All();
        void UpdateDb();
    }
}
