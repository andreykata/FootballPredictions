using System;
using System.Collections.Generic;
using System.Text;
using FootballAnalyzes.Services.Models.Games;

namespace FootballAnalyzes.Services.Admin.Models
{
    public class FootballGamePM
    {
        public int Id { get; set; }

        public DateTime MatchDate { get; set; }

        public LeaguePM League { get; set; }

        public TeamPM HomeTeam { get; set; }

        public TeamPM AwayTeam { get; set; }

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
