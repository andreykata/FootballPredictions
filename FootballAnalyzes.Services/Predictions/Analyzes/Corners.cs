namespace FootballAnalyzes.Services.Predictions.Analyzes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FootballAnalyzes.Services.Admin.Models;
    using FootballAnalyzes.Services.Models.Games;

    public class Corners : BasicAnalysis
    {
        public Corners(FootballGamePM game, List<FootballGameSM> gamesBetweenBothTeams,
            List<FootballGameSM> homeTeamGames, List<FootballGameSM> awayTeamGames,
            int allGamesCount, int allHomeOrAwayGamesCount, int lastGamesCount = 0, int lastHomeOrAwayGamesCount = 0)
            : base(game, gamesBetweenBothTeams, homeTeamGames, awayTeamGames, allGamesCount, allHomeOrAwayGamesCount, lastGamesCount, lastHomeOrAwayGamesCount)
        {
            if (this.CheckForGameStatistic())
            {
                return;
            }

            this.SumPredictCorners = 0;

            this.HTCorners();
            this.ATCorners();
            this.SumCorners();

            this.Over8Corners();
            this.Under12Corners();
        }

        public double SumPredictCorners { get; set; }

        public IEnumerable<FootballGameSM> HTGames => this.HTAllGames.Where(g => g.GameStatistic != null);
        public IEnumerable<FootballGameSM> ATGames => this.ATAllGames.Where(g => g.GameStatistic != null);
        public IEnumerable<FootballGameSM> HTHomeGames => this.HTAllHomeGames.Where(g => g.GameStatistic != null);
        public IEnumerable<FootballGameSM> ATAwayGames => this.ATAllAwayGames.Where(g => g.GameStatistic != null);

        public void HTCorners()
        {
            double hTHomeCornersAvg = HTGames.Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName).Sum(g => g.GameStatistic.HomeTeamCorners) /
                (double)HTGames.Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName).Count();
            double hTAwayCornersAvg = HTGames.Where(g => g.AwayTeam.UniqueName == this.HomeTeam.UniqueName).Sum(g => g.GameStatistic.AwayTeamCorners) /
                (double)HTGames.Where(g => g.AwayTeam.UniqueName == this.HomeTeam.UniqueName).Count();

            double aTHomeAllowedCornersAvg = ATGames.Where(g => g.HomeTeam.UniqueName == this.AwayTeam.UniqueName).Sum(g => g.GameStatistic.AwayTeamCorners) /
                (double)ATGames.Where(g => g.HomeTeam.UniqueName == this.AwayTeam.UniqueName).Count();
            double aTAwayAllowedCornersAvg = ATGames.Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName).Sum(g => g.GameStatistic.HomeTeamCorners) /
                (double)ATGames.Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName).Count();


            double hTCornersAvg = (hTHomeCornersAvg + hTAwayCornersAvg + aTHomeAllowedCornersAvg + aTAwayAllowedCornersAvg) / 4.0;
            var hTCorners = Math.Floor(hTCornersAvg) - 1;

            PredictionSM predict;

            if (!this.Predictions.Any(p => p.Name == ServiceConstants.HTCorners))
            {
                predict = new PredictionSM { Name = ServiceConstants.HTCorners, Procent = hTCorners };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.HTCorners).FirstOrDefault();
                predict.Procent = hTCorners;
            }

            this.SumPredictCorners += hTCorners;
        }

        public void ATCorners()
        {
            double hTHomeAllowedCornersAvg = HTGames.Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName).Sum(g => g.GameStatistic.AwayTeamCorners) / (double)HTGames.Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName).Count();
            double hTAwayAllowedCornersAvg = HTGames.Where(g => g.AwayTeam.UniqueName == this.HomeTeam.UniqueName).Sum(g => g.GameStatistic.HomeTeamCorners) / (double)HTGames.Where(g => g.AwayTeam.UniqueName == this.HomeTeam.UniqueName).Count();

            double aTHomeCornersAvg = ATGames.Where(g => g.HomeTeam.UniqueName == this.AwayTeam.UniqueName).Sum(g => g.GameStatistic.HomeTeamCorners) / (double)ATGames.Where(g => g.HomeTeam.UniqueName == this.AwayTeam.UniqueName).Count();
            double aTAwayCornersAvg = ATGames.Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName).Sum(g => g.GameStatistic.AwayTeamCorners) / (double)ATGames.Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName).Count();

            double aTCornersAvg = (hTHomeAllowedCornersAvg + hTAwayAllowedCornersAvg + aTHomeCornersAvg + aTAwayCornersAvg) / 4.0;
            double aTCorners = Math.Floor(aTCornersAvg) - 1;

            PredictionSM predict;

            if (!this.Predictions.Any(p => p.Name == ServiceConstants.ATCorners))
            {
                predict = new PredictionSM { Name = ServiceConstants.ATCorners, Procent = aTCorners };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.ATCorners).FirstOrDefault();
                predict.Procent = aTCorners;
            }

            this.SumPredictCorners += aTCorners;
        }

        public void SumCorners()
        {
            double sum = this.SumPredictCorners;

            PredictionSM predict;

            if (!this.Predictions.Any(p => p.Name == ServiceConstants.SumCorners))
            {
                predict = new PredictionSM { Name = ServiceConstants.SumCorners, Procent = sum };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.SumCorners).FirstOrDefault();
                predict.Procent = sum;
            }

            this.SumPredictCorners = sum;
        }

        private void Over8Corners()
        {
            var hTAllGamesCorners = this.HTGames.Where(g => (g.GameStatistic.HomeTeamCorners + g.GameStatistic.AwayTeamCorners) > 8).Count() /
                (double)this.HTGames.Count() * 100;
            var hTHomeGamesCorners = this.HTHomeGames.Where(g => (g.GameStatistic.HomeTeamCorners + g.GameStatistic.AwayTeamCorners) > 8).Count() /
                (double)this.HTHomeGames.Count() * 100;
            double hTOver8Corners = hTAllGamesCorners * 0.3 + hTHomeGamesCorners * 0.7;

            var aTAllGamesCorners = this.ATGames.Where(g => (g.GameStatistic.HomeTeamCorners + g.GameStatistic.AwayTeamCorners) > 8).Count() /
                (double)this.ATGames.Count() * 100;
            var aTAwayGamesCorners = this.ATAwayGames.Where(g => (g.GameStatistic.HomeTeamCorners + g.GameStatistic.AwayTeamCorners) > 8).Count() /
                (double)this.ATAwayGames.Count() * 100;
            double aTOver8Corners = aTAllGamesCorners * 0.3 + aTAwayGamesCorners * 0.7;

            double over8Corners = (hTOver8Corners + aTOver8Corners) / 2.0;

            //if (this.SumPredictCorners < 8)
            //{
            //    over8Corners -= 20;
            //}

            PredictionSM predict;

            if (!this.Predictions.Any(p => p.Name == ServiceConstants.Over8Corners))
            {
                predict = new PredictionSM { Name = ServiceConstants.Over8Corners, Procent = over8Corners };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.Over8Corners).FirstOrDefault();
                predict.Procent = over8Corners;
            }
        }

        private void Under12Corners()
        {
            var hTAllGamesCorners = this.HTGames.Where(g => (g.GameStatistic.HomeTeamCorners + g.GameStatistic.AwayTeamCorners) < 12).Count() /
                (double)this.HTGames.Count() * 100;
            var hTHomeGamesCorners = this.HTHomeGames.Where(g => (g.GameStatistic.HomeTeamCorners + g.GameStatistic.AwayTeamCorners) < 12).Count() /
                (double)this.HTHomeGames.Count() * 100;
            double hTUnder12Corners = hTAllGamesCorners * 0.3 + hTHomeGamesCorners * 0.7;

            var aTAllGamesCorners = this.ATGames.Where(g => (g.GameStatistic.HomeTeamCorners + g.GameStatistic.AwayTeamCorners) < 12).Count() /
                (double)this.ATGames.Count() * 100;
            var aTAwayGamesCorners = this.ATAwayGames.Where(g => (g.GameStatistic.HomeTeamCorners + g.GameStatistic.AwayTeamCorners) < 12).Count() /
                (double)this.ATAwayGames.Count() * 100;
            double aTUnder12Corners = aTAllGamesCorners * 0.3 + aTAwayGamesCorners * 0.7;

            double under12Corners = (hTUnder12Corners + aTUnder12Corners) / 2.0;

            PredictionSM predict;

            if (!this.Predictions.Any(p => p.Name == ServiceConstants.Under12Corners))
            {
                predict = new PredictionSM { Name = ServiceConstants.Under12Corners, Procent = under12Corners };
                this.Predictions.Add(predict);
            }
            else
            {
                predict = this.Predictions.Where(p => p.Name == ServiceConstants.Under12Corners).FirstOrDefault();
                predict.Procent = under12Corners;
            }
        }

        public bool CheckForGameStatistic()
        {
            if (HTGames.Count() < 5 || ATGames.Count() < 5 ||
                HTHomeGames.Count() < 5 || ATAwayGames.Count() < 5 ||
                HTGames.Where(g => g.HomeTeam.UniqueName == this.HomeTeam.UniqueName).Count() < 1 ||
                HTGames.Where(g => g.AwayTeam.UniqueName == this.HomeTeam.UniqueName).Count() < 1 ||
                ATGames.Where(g => g.HomeTeam.UniqueName == this.AwayTeam.UniqueName).Count() < 1 ||
                ATGames.Where(g => g.AwayTeam.UniqueName == this.AwayTeam.UniqueName).Count() < 1)
            {
                return true;
            }

            return false;
        }
    }
}
