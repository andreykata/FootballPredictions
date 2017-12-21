using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalyzes.Services.Admin
{
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
