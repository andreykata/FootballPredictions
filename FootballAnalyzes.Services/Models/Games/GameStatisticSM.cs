namespace FootballAnalyzes.Services.Models.Games
{
    public class GameStatisticSM
    {
        public int HomeTeamCorners { get; set; }
        public int AwayTeamCorners { get; set; }
        public int HomeTeamShotsOnTarget { get; set; }
        public int AwayTeamShotsOnTarget { get; set; }
        public int HomeTeamShotsWide { get; set; }
        public int AwayTeamShotsWide { get; set; }
        public int HomeTeamFouls { get; set; }
        public int AwayTeamFouls { get; set; }
        public int HomeTeamOffsides { get; set; }
        public int AwayTeamOffsides { get; set; }

        public override string ToString()
        {
            return $"{this.HomeTeamCorners.ToString().PadLeft(3, ' ')},{this.AwayTeamCorners.ToString().PadLeft(2, ' ')}," +
                $"{this.HomeTeamShotsOnTarget.ToString().PadLeft(2, ' ')},{this.AwayTeamShotsOnTarget.ToString().PadLeft(2, ' ')}," +
                $"{this.HomeTeamShotsWide.ToString().PadLeft(2, ' ')},{this.AwayTeamShotsWide.ToString().PadLeft(2, ' ')}," +
                $"{this.HomeTeamFouls.ToString().PadLeft(2, ' ')},{this.AwayTeamFouls.ToString().PadLeft(2, ' ')}," +
                $"{this.HomeTeamOffsides.ToString().PadLeft(2, ' ')},{this.AwayTeamOffsides.ToString().PadLeft(2, ' ')}";
        }
    }
}
