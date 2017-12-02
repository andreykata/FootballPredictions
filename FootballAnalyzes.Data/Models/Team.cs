﻿namespace FootballAnalyzes.Data.Models
{
    using System.Collections.Generic;

    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UniqueName { get; set; }

        public ICollection<FootballGame> Games { get; set; } = new List<FootballGame>();
    }
}
