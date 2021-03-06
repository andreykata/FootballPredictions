﻿namespace FootballAnalyzes.Services.Models.Games
{
    public class PredictionSM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Procent { get; set; }

        public string Result { get; set; }
        
        public int GameId { get; set; }

        public override string ToString()
        {
            return $"{this.Name}:{this.Procent:0.00}";
        }
    }
}
