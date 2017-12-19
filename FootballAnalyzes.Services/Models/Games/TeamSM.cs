namespace FootballAnalyzes.Services.Models.Games
{
    public class TeamSM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UniqueName { get; set; }

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
