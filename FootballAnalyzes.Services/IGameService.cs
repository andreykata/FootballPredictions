namespace FootballAnalyzes.Services
{
    using System;
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Models.Games;

    public interface IGameService
    {
        IEnumerable<FootballGameSM> All(int page = 1);
        IEnumerable<FootballGameSM> Next(int page = 1);
        IEnumerable<FootballGameSM> WithoutResult(DateTime date, int page = 1);
        IEnumerable<ByDateSM> GroupByDate(int page = 1);
        int TotalGamesCount();
        int TotalNextGamesCount();
        int TotalByDateCount();
        int TotalCurrnetDateGamesCount(DateTime date);
        EditGameSM FindGameForEdit(int id);
        bool EditGame(int id, int homeTeamGoals, int awayTeamGoals);
    }
}
