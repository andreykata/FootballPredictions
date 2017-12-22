namespace FootballAnalyzes.Tests.Controllers
{
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using FootballAnalyzes.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class GamesControllerTest
    {
        [Fact]
        public void IndexShouldBeOnlyForAuthorizedUsers()
        {
            // Arrange
            var methods = typeof(GamesController).GetMethods();

            // Act
            var attributes = methods
                .Where(m => m.Name == nameof(GamesController.EditGame))
                .FirstOrDefault()
                .GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }
    }
}
