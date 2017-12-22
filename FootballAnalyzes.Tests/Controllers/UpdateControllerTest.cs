namespace FootballAnalyzes.Tests.Controllers
{
    using System.Linq;
    using FluentAssertions;
    using FootballAnalyzes.Web.Areas.Admin.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Xunit;

    public class UpdateControllerTest
    {
        [Fact]
        public void IndexShouldBeOnlyForAuthorizedUsers()
        {
            // Arrange
            var controller = typeof(UpdateController);

            // Act
            var attributes = controller.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }
    }
}
