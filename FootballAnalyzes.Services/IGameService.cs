namespace FootballAnalyzes.Services
{
    using System;
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Admin.Models;
    using FootballAnalyzes.Services.Models.Games;

    public interface IGameService
    {
        IEnumerable<FootballGamePM> All(int page = 1);
        IEnumerable<FootballGamePM> Next(int page = 1);
        IEnumerable<FootballGameSM> WithoutResult(DateTime date, int page = 1);
        IEnumerable<FootballGamePM> DateGames(DateTime date, int page = 1);
        IEnumerable<FootballGamePM> TeamGames(DateTime matchDate, int teamId, int page = 1);
        IEnumerable<FootballGamePM> BetweenBothTeams(DateTime matchDate, int homeTeamId, int awayTeamId);
        IEnumerable<ByDateSM> GroupByDate(int page = 1);
        FootballGamePM ById(int gameId);
        int TotalGamesCount();
        int TotalNextGamesCount();
        int TotalByDateCount();
        int TotalCurrnetDateGamesCount(DateTime date);
        EditGameSM FindGameForEdit(int id);
        bool EditGame(int id, int homeTeamGoals, int awayTeamGoals);
    }
}
