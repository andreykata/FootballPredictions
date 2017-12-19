namespace FootballAnalyzes.Web.Infrastructure.Extensions
{
    using System;
    using System.Threading.Tasks;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Data.Models;
    using FootballAnalyzes.UpdateDatabase;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<FootballAnalyzesDbContext>().Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var db = serviceScope.ServiceProvider.GetService<FootballAnalyzesDbContext>();

                Task
                    .Run(async () =>
                    {                        
                        await AddAdminUser(userManager, roleManager);


                        if (await db.FootballGames.CountAsync() == 0)
                        {
                            var updateDb = new StartUpdate(db);
                            updateDb.SeedOldGames();
                        }

                    })
                    .Wait();
            }

            return app;
        }

        private static async Task AddAdminUser(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminName = WebConstants.AdministratorRole;

            var roles = new[]
            {
                adminName
            };

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role
                    });
                }
            }

            var adminEmail = "admin@abv.bg";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    Email = adminEmail,
                    UserName = adminName,
                    Name = adminName,
                    Birthdate = DateTime.UtcNow
                };

                await userManager.CreateAsync(adminUser, "admin12");

                await userManager.AddToRoleAsync(adminUser, adminName);
            }
        }
    }
}
