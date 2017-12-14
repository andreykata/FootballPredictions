using System;
using System.Collections.Generic;
using System.Text;
using FootballAnalyzes.Data.Models;

namespace FootballAnalyzes.UpdateDatabase.BindingModels
{
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
