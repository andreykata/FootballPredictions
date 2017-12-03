namespace FootballAnalyzes.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class FootballGame
    {
        public int Id { get; set; }

        public DateTime MatchDate { get; set; }

        public int LeagueId { get; set; }

        public League League { get; set; }

        public int HomeTeamId { get; set; }
        
        [ForeignKey("HomeTeamId")]
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        [ForeignKey("AwayTeamId")]
        public Team AwayTeam { get; set; }
        
        public int FullTimeResultId { get; set; }

        [ForeignKey("FullTimeResultId")]
        public GameResult FullTimeResult { get; set; }

        public int FirstHalfResultId { get; set; }

        [ForeignKey("FirstHalfResultId")]
        public GameResult FirstHalfResult { get; set; }

        public int GameStatisticId { get; set; }

        [ForeignKey("GameStatisticId")]
        public GameStatistic GameStatistic { get; set; }

        public ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
    }
}
