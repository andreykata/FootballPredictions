﻿namespace FootballAnalyzes.Data.Models
{
    public class GameStatistic
    {
        public int Id { get; set; }
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
            return $"{this.HomeTeamCorners},{this.AwayTeamCorners},{this.HomeTeamShotsOnTarget},{this.AwayTeamShotsOnTarget}," +
                $"{this.HomeTeamShotsWide},{this.AwayTeamShotsWide},{this.HomeTeamFouls},{this.AwayTeamFouls},{this.HomeTeamOffsides},{this.AwayTeamOffsides}";
        }
    }
}
