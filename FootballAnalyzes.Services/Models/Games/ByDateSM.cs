namespace FootballAnalyzes.Services.Models.Games
{
    using System;

    public class ByDateSM
    {
        public DateTime GamesDate { get; set; }
        public int GamesWithResultCount { get; set; }
        public int GamesWithoutResultCount { get; set; }
        public int TotalGamesCount => this.GamesWithResultCount + this.GamesWithoutResultCount;
    }
}
