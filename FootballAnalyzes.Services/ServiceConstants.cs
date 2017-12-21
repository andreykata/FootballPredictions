using FootballAnalyzes.Data.Models;

namespace FootballAnalyzes.Services
{
    public static class ServiceConstants
    {
        public const int GamesPageSize = 100;
        public const int GamesByDatePageSize = 20;
        public const int GameDetailsPageSize = 10;

        public const int ReturnMonthsForOldGames = -1;
        public const int LastOldGamesCount = 750;

        public const string DateStringFormat = "dd-MM-yyyy";

        // Prediction constants
        public const string OverTwoGoals = "OverTwoGoals";
        public const string MaxTwoGoals = "MaxTwoGoals";
        public const string GoalGoal = "GoalGoal";
        public const string HTGoals = "HTGoals";
        public const string ATGoals = "ATGoals";
        public const string Over8Corners = "Over8Corners";
        public const string Over12Corners = "Over12Corners";
        public const string Under8Corners = "Under8Corners";
        public const string Under12Corners = "Under12Corners";
        public const string Result1X2 = "Result1X2";
        public const string HTCorners = "HTCorners";
        public const string ATCorners = "ATCorners";
        public const string SumCorners = "SumCorners";
        public const string HTWin = "HTWin";
        public const string ATWin = "ATWin";
        public const string Draw = "Draw";
        public const string HTWinBetweenBothTeams = "HTWinBetweenBothTeams";
        public const string ATWinBetweenBothTeams = "ATWinBetweenBothTeams";

        public const string DrawByShots = "DrawByShots";
        public const string Divide = "Divide";
        public const string OverZeroGoalFirstHalf = "OverZeroGoalFirstHalf";
        public const string OverTwoGoalFirstHalf = "OverTwoGoalFirstHalf";
        public const string MaxZeroGoalFirstHalf = "MaxZeroGoalFirstHalf";
        public const string OverZeroGoalSecondHalf = "OverZeroGoalSecondHalf";

        public const string Yes = "Yes";
        public const string No = "No";

        public const string PastGames = "PastGames";
        public const string NextGames = "NextGames";
    }
}
