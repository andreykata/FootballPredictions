namespace FootballAnalyzes.Tests.Services
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Data.Models;
    using FootballAnalyzes.Services.Implementations;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class GameServiceTest
    {
        public GameServiceTest()
        {
            TestStartup.Initialize();
        }

        [Fact]
        public void ShouldReturnGamesWithoutResultAndOrder()
        {
            // Arrange     
            var db = GetDatebase();

            var league = new League { Id = 1, Country = "Uk", Name = "Premier league", Year = "2018", Stage = "" };
            var team = new Team { Id = 1, Name = "Man Utd", UniqueName = "Manchester United"};

            var firstGame = new FootballGame { Id = 1, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team };
            var secondGame = new FootballGame { Id = 2, MatchDate = DateTime.UtcNow, FullTimeResult = new GameResult { Id = 2, Result = ResultEnum.H, HomeTeamGoals = 3, AwayTeamGoals = 1 } };
            var thirdGame = new FootballGame { Id = 3, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team };

            db.AddRange(firstGame, secondGame, thirdGame);
            db.SaveChanges();

            var gameService = new GameService(db);
            // Act
            var result = gameService.WithoutResult(DateTime.UtcNow);

            // Assert
            result
                .Should()
                .Match(r =>
                    r.ElementAt(0).Id == 3 &&
                    r.ElementAt(1).Id == 1)
                .And
                .HaveCount(2);
        }

        [Fact]
        public void ShouldReturnGameById()
        {
            // Arrange     
            var db = GetDatebase();

            var league = new League { Id = 1, Country = "Uk", Name = "Premier league", Year = "2018", Stage = "" };
            var team = new Team { Id = 2, Name = "Man Utd", UniqueName = "Manchester United" };

            var firstGame = new FootballGame { Id = 1, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team };
            var secondGame = new FootballGame { Id = 2, MatchDate = DateTime.UtcNow, FullTimeResult = new GameResult { Id = 2, Result = ResultEnum.H, HomeTeamGoals = 3, AwayTeamGoals = 1 } };
            var thirdGame = new FootballGame { Id = 3, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team };

            db.AddRange(firstGame, secondGame, thirdGame);
            db.SaveChanges();

            var gameService = new GameService(db);
            // Act
            var result = gameService.ById(3);

            // Assert
            result
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void ShouldReturnNullGameById()
        {
            // Arrange     
            var db = GetDatebase();

            var league = new League { Id = 1, Country = "Uk", Name = "Premier league", Year = "2018", Stage = "" };
            var team = new Team { Id = 2, Name = "Man Utd", UniqueName = "Manchester United" };

            var firstGame = new FootballGame { Id = 1, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team };

            db.AddRange(firstGame);
            db.SaveChanges();

            var gameService = new GameService(db);
            // Act
            var result = gameService.ById(4);

            // Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public void ShouldReturnAllGamesAndOrder()
        {
            // Arrange     
            var db = GetDatebase();

            var league = new League { Id = 1, Country = "Uk", Name = "Premier league", Year = "2018", Stage = "" };
            var team = new Team { Id = 1, Name = "Man Utd", UniqueName = "Manchester United" };

            var firstGame = new FootballGame { Id = 1, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team };
            var secondGame = new FootballGame { Id = 2, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team, FullTimeResult = new GameResult { Id = 2, Result = ResultEnum.H, HomeTeamGoals = 3, AwayTeamGoals = 1 } };
            var thirdGame = new FootballGame { Id = 3, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team };
            var fourthGame = new FootballGame { Id = 4, MatchDate = DateTime.UtcNow.AddDays(-1), League = league, HomeTeam = team, AwayTeam = team, FullTimeResult = new GameResult { Id = 1, Result = ResultEnum.H, HomeTeamGoals = 3, AwayTeamGoals = 1 } };
            
            db.AddRange(firstGame, secondGame, thirdGame, fourthGame);
            db.SaveChanges();

            var gameService = new GameService(db);
            // Act
            var result = gameService.All();

            // Assert
            result
                .Should()
                .Match(r =>
                    r.ElementAt(0).Id == 2 &&
                    r.ElementAt(1).Id == 4)
                .And
                .HaveCount(2);
        }

        [Fact]
        public void ShouldReturnNullAll()
        {
            // Arrange     
            var db = GetDatebase();

            var league = new League { Id = 1, Country = "Uk", Name = "Premier league", Year = "2018", Stage = "" };
            var team = new Team { Id = 1, Name = "Man Utd", UniqueName = "Manchester United" };

            var firstGame = new FootballGame { Id = 1, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team };

            db.AddRange(firstGame);
            db.SaveChanges();

            var gameService = new GameService(db);
            // Act
            var result = gameService.All();

            // Assert
            result
                .Should()
                .BeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnNextGamesAndOrder()
        {
            // Arrange     
            var db = GetDatebase();

            var league = new League { Id = 1, Country = "Uk", Name = "Premier league", Year = "2018", Stage = "" };
            var team = new Team { Id = 1, Name = "Man Utd", UniqueName = "Manchester United" };

            var firstGame = new FootballGame { Id = 1, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team };
            var secondGame = new FootballGame { Id = 2, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team, FullTimeResult = new GameResult { Id = 2, Result = ResultEnum.H, HomeTeamGoals = 3, AwayTeamGoals = 1 } };
            var thirdGame = new FootballGame { Id = 3, MatchDate = DateTime.UtcNow.AddDays(1), League = league, HomeTeam = team, AwayTeam = team };
            var fourthGame = new FootballGame { Id = 4, MatchDate = DateTime.UtcNow.AddDays(1), League = league, HomeTeam = team, AwayTeam = team, FullTimeResult = new GameResult { Id = 1, Result = ResultEnum.H, HomeTeamGoals = 3, AwayTeamGoals = 1 } };

            db.AddRange(firstGame, secondGame, thirdGame, fourthGame);
            db.SaveChanges();

            var gameService = new GameService(db);
            // Act
            var result = gameService.Next();

            // Assert
            result
                .Should()
                .Match(r =>
                    r.ElementAt(0).Id == 3 &&
                    r.ElementAt(1).Id == 1)
                .And
                .HaveCount(2);
        }

        [Fact]
        public void ShouldReturnNullNext()
        {
            // Arrange     
            var db = GetDatebase();

            var league = new League { Id = 1, Country = "Uk", Name = "Premier league", Year = "2018", Stage = "" };
            var team = new Team { Id = 1, Name = "Man Utd", UniqueName = "Manchester United" };

            var firstGame = new FootballGame { Id = 1, MatchDate = DateTime.UtcNow.AddDays(-1), League = league, HomeTeam = team, AwayTeam = team };
            var secondGame = new FootballGame { Id = 2, MatchDate = DateTime.UtcNow, League = league, HomeTeam = team, AwayTeam = team, FullTimeResult = new GameResult { Id = 2, Result = ResultEnum.H, HomeTeamGoals = 3, AwayTeamGoals = 1 } };
            var thirdGame = new FootballGame { Id = 3, MatchDate = DateTime.UtcNow.AddDays(-1), League = league, HomeTeam = team, AwayTeam = team };
            var fourthGame = new FootballGame { Id = 4, MatchDate = DateTime.UtcNow.AddDays(1), League = league, HomeTeam = team, AwayTeam = team, FullTimeResult = new GameResult { Id = 1, Result = ResultEnum.H, HomeTeamGoals = 3, AwayTeamGoals = 1 } };

            db.AddRange(firstGame, secondGame, thirdGame, fourthGame);
            db.SaveChanges();

            var gameService = new GameService(db);
            // Act
            var result = gameService.Next();

            // Assert
            result
                .Should()
                .BeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnTeamGamesAndOrder()
        {
            // Arrange     
            var db = GetDatebase();

            var league = new League { Id = 1, Country = "Uk", Name = "Premier league", Year = "2018", Stage = "" };
            var team = new Team { Id = 1, Name = "Man Utd", UniqueName = "Manchester United" };

            var firstGame = new FootballGame { Id = 1, MatchDate = DateTime.UtcNow.AddDays(-1), League = league, HomeTeam = team, AwayTeam = team };
            var secondGame = new FootballGame { Id = 2, MatchDate = DateTime.UtcNow.AddDays(-2), League = league, HomeTeam = team, AwayTeam = team, FullTimeResult = new GameResult { Id = 2, Result = ResultEnum.H, HomeTeamGoals = 3, AwayTeamGoals = 1 } };
            var thirdGame = new FootballGame { Id = 3, MatchDate = DateTime.UtcNow.AddDays(1), League = league, HomeTeam = team, AwayTeam = team };
            var fourthGame = new FootballGame { Id = 4, MatchDate = DateTime.UtcNow.AddDays(1), League = league, HomeTeam = team, AwayTeam = team, FullTimeResult = new GameResult { Id = 1, Result = ResultEnum.H, HomeTeamGoals = 3, AwayTeamGoals = 1 } };

            db.AddRange(firstGame, secondGame, thirdGame, fourthGame);
            db.SaveChanges();

            var gameService = new GameService(db);
            // Act
            var result = gameService.TeamGames(DateTime.Now, 1);

            // Assert
            result
                .Should()
                .Match(r =>
                    r.ElementAt(0).Id == 2)
                .And
                .HaveCount(1);
        }

        private FootballAnalyzesDbContext GetDatebase()
        {
            var dbOptions = new DbContextOptionsBuilder<FootballAnalyzesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new FootballAnalyzesDbContext(dbOptions);
        }
    }
}
