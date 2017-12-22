namespace FootballAnalyzes.Services.Admin
{
    using System;

    public interface IAdminUpdateService
    {
        string UpdateDb(DateTime nextGamesDate);
        string UpdateOldGames(DateTime startDate, DateTime endDate);
        bool DeleteGamesByDate(string date);
        int DeleteGamesByDateCount(string date);
        string UpdateDatesInfo();
        string MakePredictionToOldGames();
        string MakePredictionToNewGames(DateTime nextGamesDate);
    }
}
