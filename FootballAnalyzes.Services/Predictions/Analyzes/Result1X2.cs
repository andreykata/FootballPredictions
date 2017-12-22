namespace FootballAnalyzes.Services.Predictions.Analyzes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FootballAnalyzes.Data.Models;
    using FootballAnalyzes.Services.Admin.Models;
    using FootballAnalyzes.Services.Models.Games;

    public class Result1X2 : BasicAnalysis
    {
        private double hTWin;
        private double aTWin;
        private double drawsBothTeams = 0;
        

        public Result1X2(FootballGamePM game, List<FootballGameSM> gamesBetweenBothTeams,
            List<FootballGameSM> homeTeamGames, List<FootballGameSM> awayTeamGames,
            int allGamesCount, int allHomeOrAwayGamesCount, int lastGamesCount = 0, int lastHomeOrAwayGamesCount = 0)
            : base(game, gamesBetweenBothTeams, homeTeamGames, awayTeamGames, allGamesCount, allHomeOrAwayGamesCount, lastGamesCount, lastHomeOrAwayGamesCount)
        {
            if (this.CheckForZeroCount())
            {
                return;
            }

            this.GamesBothTeams();
            this.Result1();
            this.Result2();
            this.DivideBetweenHomeAndAway();
            this.ResultDraw();
        }

        public double Divide { get; set; }

        public void Result1()
        {
            double hTAllGamesWins = this.HTAllGames
                .Where(g => (g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H) ||
                (g.AwayTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A))
                .Count() / (double)this.MinHTAllGamesCount * 100;
            double hTAllHomeGamesWins = this.HTAllHomeGames
                .Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H)
                .Count() / (double)this.MinHTAllHomeGamesCount * 100;
            double hTLastGamesWins = this.HTLastGames
                .Where(g => (g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H) ||
                (g.AwayTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A))
                .Count() / (double)this.MinHTLastGamesCount * 100;
            double hTLastHomeGamesWins = this.HTLastHomeGames
                 .Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H)
                .Count() / (double)this.MinHTLastHomeGamesCount * 100;

            double hTWin = hTAllGamesWins * 0.4 + hTAllHomeGamesWins * 0.1 + hTLastGamesWins * 0.4 + hTLastHomeGamesWins * 0.1;

            double aTAllGamesLosses = this.ATAllGames
                .Where(g => (g.HomeTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A) ||
                (g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H))
                .Count() / (double)this.MinATAllGamesCount * 100;
            double aTAllAwayGamesLosses = this.ATAllAwayGames
                .Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H)
                .Count() / (double)this.MinATAllAwayGamesCount * 100;
            double aTLastGamesLosses = this.ATLastGames
                .Where(g => (g.HomeTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A) ||
                (g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H))
                .Count() / (double)this.MinATLastGamesCount * 100;
            double aTLastAwayGamesLosses = this.ATLastAwayGames
                .Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H)
                .Count() / (double)this.MinATLastAwayGamesCount * 100;

            double aTLoss = aTAllGamesLosses * 0.4 + aTAllAwayGamesLosses * 0.1 + aTLastGamesLosses * 0.4 + aTLastAwayGamesLosses * 0.1;

            double procent = (hTWin + aTLoss) / 2.0;

            PredictionSM predict;

            if (this.Predictions.Count() == 0 || !this.Predictions.Any(p => p.Name == ServiceConstants.HTWin))
            {
                predict = new PredictionSM { Name = ServiceConstants.HTWin, Procent = procent };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.HTWin).FirstOrDefault();
                predict.Procent = procent;
            }

            this.hTWin = procent;
        }

        public void Result2()
        {
            double hTAllGamesLosses = this.HTAllGames.
                Where(g => (g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A) ||
                (g.AwayTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H))
                .Count() / (double)MinHTAllGamesCount * 100;
            double hTAllHomeGamesLosses = this.HTAllHomeGames
                .Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A)
                .Count() / (double)this.MinHTAllHomeGamesCount * 100;
            double hTLastGamesLosses = this.HTLastGames
                .Where(g => (g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A) ||
                (g.AwayTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H))
                .Count() / (double)this.MinHTLastGamesCount * 100;
            double hTLastHomeGamesLosses = this.HTLastHomeGames
                 .Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A)
                .Count() / (double)this.MinHTLastHomeGamesCount * 100;

            double hTLosses = (hTAllGamesLosses + hTAllHomeGamesLosses + hTLastGamesLosses + hTLastHomeGamesLosses) / 4.0;

            double aTAllGamesWins = this.ATAllGames
               .Where(g => (g.HomeTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H) ||
               (g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A))
               .Count() / (double)this.MinATAllGamesCount * 100;
            double aTAllAwayGamesWins = this.ATAllAwayGames
                .Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A)
                .Count() / (double)this.MinATAllAwayGamesCount * 100;
            double aTLastGamesWins = this.ATLastGames
                .Where(g => (g.HomeTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H) ||
                (g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A))
                .Count() / (double)this.MinATLastGamesCount * 100;
            double aTLastAwayGamesWins = this.ATLastAwayGames
                .Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A)
                .Count() / (double)this.MinATLastAwayGamesCount * 100;

            double aTWins = (aTAllGamesWins + aTAllAwayGamesWins + aTLastGamesWins + aTLastAwayGamesWins) / 4.0;

            double procent = (hTLosses + aTWins) / 2.0;

            PredictionSM predict;

            if (this.Predictions.Count() == 0 || !this.Predictions.Any(p => p.Name == ServiceConstants.ATWin))
            {
                predict = new PredictionSM { Name = ServiceConstants.ATWin, Procent = procent };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.ATWin).FirstOrDefault();
                predict.Procent = procent;
            }

            this.aTWin = procent;
        }

        public void ResultDraw()
        {
            double hTAllGamesDraws = this.HTAllGames
                .Where(g => (g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D) ||
                (g.AwayTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D))
                .Count() / (double)this.MinHTAllGamesCount * 100;
            double hTAllHomeGameDraws = this.HTAllHomeGames
                .Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D)
                .Count() / (double)this.MinHTAllHomeGamesCount * 100;
            double hTLastGamesDraws = this.HTLastGames
                .Where(g => (g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D) ||
                (g.AwayTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D))
                .Count() / (double)this.MinHTLastGamesCount * 100;
            double hTLastHomeGamesDraws = this.HTLastHomeGames
                 .Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D)
                .Count() / (double)this.MinHTLastHomeGamesCount * 100;

            double hTDraw = hTAllGamesDraws * 0.3 + hTAllHomeGameDraws * 0.3 + hTLastGamesDraws * 0.2 + hTLastHomeGamesDraws * 0.2;

            double aTAllGamesDraws = this.ATAllGames
                .Where(g => (g.HomeTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D) ||
                (g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D))
                .Count() / (double)this.MinATAllGamesCount * 100;
            double aTAllAwayGamesDraws = this.ATAllAwayGames
                .Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D)
                .Count() / (double)this.MinATAllAwayGamesCount * 100;
            double aTLastGamesDraws = this.ATLastGames
                .Where(g => (g.HomeTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D) ||
                (g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D))
                .Count() / (double)this.MinATLastGamesCount * 100;
            double aTLastAwayGamesDraws = this.ATLastAwayGames
                .Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D)
                .Count() / (double)this.MinATLastAwayGamesCount * 100;

            double aTDraw = aTAllGamesDraws * 0.3 + aTAllAwayGamesDraws * 0.3 + aTLastGamesDraws * 0.2 + aTLastAwayGamesDraws * 0.2;

            double procent = hTDraw * 0.5 + aTDraw * 0.5;

            // Games between the both teams
            if (this.drawsBothTeams > 0)
            {
                procent += this.drawsBothTeams * 0.2;
            }

            PredictionSM predict;

            if (this.Predictions.Count() == 0 || !this.Predictions.Any(p => p.Name == ServiceConstants.Draw))
            {
                predict = new PredictionSM { Name = ServiceConstants.Draw, Procent = procent };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.Draw).FirstOrDefault();
                predict.Procent = procent;
            }
        }

        public void DivideBetweenHomeAndAway()
        {
            double divide = Math.Abs(this.hTWin - this.aTWin);

            PredictionSM predict;

            if (this.Predictions.Count() == 0 || !this.Predictions.Any(p => p.Name == ServiceConstants.Divide))
            {
                predict = new PredictionSM { Name = ServiceConstants.Divide, Procent = divide };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.Divide).FirstOrDefault();
                predict.Procent = divide;
            }

            this.Divide = divide;
        }

        public void GamesBothTeams()
        {
            if (this.GamesBetweenBothTeams == null || this.GamesBetweenBothTeams.Count() < 5 ||
                this.GamesBetweenBothTeams.Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName).Count() < 1)
            {
                return;
            }

            int allGamesCount = this.GamesBetweenBothTeams.Count();
            int hTHomeGamesCount = this.GamesBetweenBothTeams.Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName).Count();

            // Home team wins between both teams
            double hTAllGamesWins = this.GamesBetweenBothTeams
                .Where(g => (g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H) ||
                (g.AwayTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A))
                .Count() / (double)allGamesCount * 100;
            double hTAllHomeGamesWins = this.GamesBetweenBothTeams
                .Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H)
                .Count() / (double)hTHomeGamesCount * 100;

            double hTWin = hTAllGamesWins * 0.8 + hTAllHomeGamesWins * 0.2;

            PredictionSM predict;

            if (this.Predictions.Count() == 0 || !this.Predictions.Any(p => p.Name == ServiceConstants.HTWinBetweenBothTeams))
            {
                predict = new PredictionSM { Name = ServiceConstants.HTWinBetweenBothTeams, Procent = hTWin };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.HTWinBetweenBothTeams).FirstOrDefault();
                predict.Procent = hTWin;
            }

            // Away team wins between both teams
            double aTAllGamesWins = this.GamesBetweenBothTeams
                .Where(g => (g.HomeTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.H) ||
                (g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A))
                .Count() / (double)allGamesCount * 100;
            double aTAllAwayGamesWins = this.GamesBetweenBothTeams
                .Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.A)
                .Count() / (double)hTHomeGamesCount * 100;

            double aTWin = aTAllGamesWins * 0.8 + aTAllAwayGamesWins * 0.2;

            if (this.Predictions.Count() == 0 || !this.Predictions.Any(p => p.Name == ServiceConstants.ATWinBetweenBothTeams))
            {
                predict = new PredictionSM { Name = ServiceConstants.ATWinBetweenBothTeams, Procent = aTWin };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.ATWinBetweenBothTeams).FirstOrDefault();
                predict.Procent = aTWin;
            }

            // Draws
            var allGameDrawsProcent = this.GamesBetweenBothTeams.Where(g => g.FullTimeResult.Result == ResultEnum.D).Count() / (double)allGamesCount * 100;
            var homeGameDrawsProcent = this.GamesBetweenBothTeams
                .Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName && g.FullTimeResult.Result == ResultEnum.D).Count() / (double)hTHomeGamesCount * 100;

            this.drawsBothTeams = allGameDrawsProcent * 0.1 + homeGameDrawsProcent * 0.9;
        }

        public bool CheckForZeroCount()
        {
            if (this.MinHTAllGamesCount < 7 || this.MinHTAllHomeGamesCount <= 3 || this.MinHTLastGamesCount <= 3 || this.MinHTLastHomeGamesCount <= 3 ||
                this.MinATAllGamesCount < 7 || this.MinATAllAwayGamesCount <= 3 || this.MinATLastGamesCount <= 3 || this.MinATLastAwayGamesCount <= 3)
            {
                return true;
            }

            return false;
        }
        
    }
}
