namespace FootballAnalyzes.UpdateDatabase.BindingModels
{
    public class LeagueBM
    {
        public string Type { get; set; }

        public string Country { get; set; }

        public string Name { get; set; }

        public string Year { get; set; }

        public string Stage { get; set; }

        public string UniqueName => $"{Country},{Name},{Year},{Stage},{Type}";
    }
}
