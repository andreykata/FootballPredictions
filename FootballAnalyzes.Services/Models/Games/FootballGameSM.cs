namespace FootballAnalyzes.Services.Models.Games
{
    using System;

    public class FootballGameSM
    {
        public int Id { get; set; }

        public DateTime MatchDate { get; set; }
        
        public LeagueSM League { get; set; }
        
        public TeamSM HomeTeam { get; set; }
        
        public TeamSM AwayTeam { get; set; }
        
        public GameResultSM FullTimeResult { get; set; }
        
        public GameResultSM FirstHalfResult { get; set; }
        
        public GameStatisticSM GameStatistic { get; set; }

        public override string ToString()
        {
            string result = $"{this.MatchDate.ToString("yyyyMMdd HH:mm")},{this.League},{this.HomeTeam},{this.AwayTeam}";

            if (this.FullTimeResult != null)
            {
                result += $",{this.FullTimeResult}";
            }
            if (this.FirstHalfResult != null)
            {
                result += $",{this.FirstHalfResult}";
            }
            if (this.GameStatistic != null)
            {
                result += $",{this.GameStatistic}";
            }

            return result;
        }
    }
}
