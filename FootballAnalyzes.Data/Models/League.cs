namespace FootballAnalyzes.Data.Models
{
    using System.Collections.Generic;

    public class League
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Country { get; set; }

        public string Name { get; set; }

        public string Year { get; set; }

        public string Stage { get; set; }

        public string UniqueName { get; set; }

        public ICollection<FootballGame> Games { get; set; } = new List<FootballGame>();
    }
}
