namespace FootballAnalyzes.Services.Models.Games
{
    public class LeagueSM
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Country { get; set; }

        public string Name { get; set; }

        public string Year { get; set; }

        public string Stage { get; set; }

        public string UniqueName { get; set; }

        public override string ToString()
        {
            return $"{this.UniqueName}";
        }
    }
}
