using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalyzes.UpdateDatabase.BindingModels
{
    public class FootballGameBM
    {
        public DateTime MatchDate { get; set; }

        public int LeagueId { get; set; }

        public LeagueBM League { get; set; }

        public int HomeTeamId { get; set; }
        
        public TeamBM HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        
        public TeamBM AwayTeam { get; set; }

        public int? FullTimeResultId { get; set; }
        
        public GameResultBM FullTimeResult { get; set; }

        public int? FirstHalfResultId { get; set; }
        
        public GameResultBM FirstHalfResult { get; set; }

        public int? GameStatisticId { get; set; }
        
        public GameStatisticBM GameStatistic { get; set; }
    }
}
