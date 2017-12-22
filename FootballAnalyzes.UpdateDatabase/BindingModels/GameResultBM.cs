namespace FootballAnalyzes.UpdateDatabase.BindingModels
{
    using FootballAnalyzes.Data.Models;

    public class GameResultBM
    {
        public ResultEnum Result { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public int GameId { get; set; }
        public FootballGameBM Game { get; set; }

        public override string ToString()
        {
            return $"{this.Result.ToString()},{this.HomeTeamGoals}:{this.AwayTeamGoals}";
        }
    }
}
