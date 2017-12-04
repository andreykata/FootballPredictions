namespace FootballAnalyzes.Services.Models.Games
{
    using FootballAnalyzes.Data.Models;

    public class GameResultSM
    {
        public ResultEnum Result { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
    }
}
