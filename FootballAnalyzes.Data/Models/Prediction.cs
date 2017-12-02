namespace FootballAnalyzes.Data.Models
{
    public class Prediction
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Procent { get; set; }

        public string Result { get; set; }

        public bool Selected { get; set; }

        public int GameId { get; set; }

        public FootballGame Game { get; set; }
    }
}
