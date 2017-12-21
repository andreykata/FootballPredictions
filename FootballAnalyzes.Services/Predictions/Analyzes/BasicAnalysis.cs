namespace FootballAnalyzes.Services.Predictions.Analyzes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FootballAnalyzes.Services.Admin.Models;
    using FootballAnalyzes.Services.Models.Games;

    public class BasicAnalysis
    {
        private int allGamesCount;
        private int allHomeOrAwayGamesCount;
        private int lastGamesCount;
        private int lastHomeOrAwayGamesCount;
        private List<FootballGameSM> gamesBetweenBothTeams;
        private List<FootballGameSM> homeTeamGames;
        private List<FootballGameSM> awayTeamGames;
        
        public BasicAnalysis(FootballGamePM game, List<FootballGameSM> gamesBetweenBothTeams,
            List<FootballGameSM> homeTeamGames, List<FootballGameSM> awayTeamGames,
            int allGamesCount, int allHomeOrAwayGamesCount, int lastGamesCount = 0, int lastHomeOrAwayGamesCount = 0)
        {
            this.Game = game;
            this.gamesBetweenBothTeams = gamesBetweenBothTeams;
            this.homeTeamGames = homeTeamGames;
            this.awayTeamGames = awayTeamGames;
            this.allGamesCount = allGamesCount;
            this.allHomeOrAwayGamesCount = allHomeOrAwayGamesCount;
            this.lastGamesCount = lastGamesCount;
            this.lastHomeOrAwayGamesCount = lastHomeOrAwayGamesCount;
        }

        protected FootballGamePM Game { get; set; }
        protected TeamSM HomeTeam => this.Game.HomeTeam;
        protected TeamSM AwayTeam => this.Game.AwayTeam;        
        protected IEnumerable<FootballGameSM> HomeTeamFootballGames => this.homeTeamGames.Where(g => !g.League.Name.Contains("friend")).Reverse();
        protected IEnumerable<FootballGameSM> AwayTeamFootballGames => this.awayTeamGames.Where(g => !g.League.Name.Contains("friend")).Reverse();

        protected IEnumerable<FootballGameSM> HTAllGames => this.HomeTeamFootballGames.Take(this.allGamesCount);
        protected IEnumerable<FootballGameSM> HTAllHomeGames => this.HomeTeamFootballGames.Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName).Take(this.allHomeOrAwayGamesCount);
        protected IEnumerable<FootballGameSM> HTLastGames => this.HTAllGames.Take(this.lastGamesCount);
        protected IEnumerable<FootballGameSM> HTLastHomeGames => this.HTAllHomeGames.Take(this.lastHomeOrAwayGamesCount);
                                          
        protected IEnumerable<FootballGameSM> ATAllGames => this.AwayTeamFootballGames.Take(this.allGamesCount);
        protected IEnumerable<FootballGameSM> ATAllAwayGames => this.AwayTeamFootballGames.Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName).Take(this.allHomeOrAwayGamesCount);
        protected IEnumerable<FootballGameSM> ATLastGames => this.ATAllGames.Take(this.lastGamesCount);
        protected IEnumerable<FootballGameSM> ATLastAwayGames => this.ATAllAwayGames.Take(this.lastHomeOrAwayGamesCount);
                                          
        protected IEnumerable<FootballGameSM> GamesBetweenBothTeams => this.gamesBetweenBothTeams;

        protected int MinHTAllGamesCount => Math.Min(this.allGamesCount, this.HTAllGames.Count());
        protected int MinHTAllHomeGamesCount => Math.Min(this.allHomeOrAwayGamesCount, this.HTAllHomeGames.Count());
        protected int MinHTLastGamesCount => Math.Min(this.lastGamesCount, this.HTLastGames.Count());
        protected int MinHTLastHomeGamesCount => Math.Min(this.lastHomeOrAwayGamesCount, this.HTLastHomeGames.Count());

        protected int MinATAllGamesCount => Math.Min(this.allGamesCount, this.ATAllGames.Count());
        protected int MinATAllAwayGamesCount => Math.Min(this.allHomeOrAwayGamesCount, this.ATAllAwayGames.Count());
        protected int MinATLastGamesCount => Math.Min(this.lastGamesCount, this.ATLastGames.Count());
        protected int MinATLastAwayGamesCount => Math.Min(this.lastHomeOrAwayGamesCount, this.ATLastAwayGames.Count());

        public List<PredictionSM> Predictions => this.Game.Predictions;
    }
}
