using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalyzes.Services.Models.Games
{
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
    }
}
