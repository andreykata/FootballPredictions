namespace FootballAnalyzes.Data.Models
{
    public class GameResult
    {
        public int Id { get; set; }
        public ResultEnum Result { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }

        public override string ToString()
        {
            return $"{this.Result.ToString()},{this.HomeTeamGoals}:{this.AwayTeamGoals}";
        }
    }
}
