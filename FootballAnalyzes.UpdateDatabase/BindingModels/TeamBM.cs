﻿namespace FootballAnalyzes.UpdateDatabase.BindingModels
{
    public class TeamBM
    {
        public string Name { get; set; }
        public string UniqueName { get; set; }

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
