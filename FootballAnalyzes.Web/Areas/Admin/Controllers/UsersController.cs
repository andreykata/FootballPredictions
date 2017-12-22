namespace FootballAnalyzes.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using FootballAnalyzes.Data.Models;
    using FootballAnalyzes.Services.Admin;
    using FootballAnalyzes.Web.Areas.Admin.Models.Users;
    using FootballAnalyzes.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UsersController : BaseAdminController
    {
        private readonly IAdminUserService users;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UsersController(IAdminUserService users, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.users = users;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = this.users.All();

            foreach (var user in users)
            {
                var currentUser = await this.userManager.FindByIdAsync(user.Id);
                var userRoles = await this.userManager.GetRolesAsync(currentUser);
                user.CurrentRole = string.Join(", ", userRoles);
            }

            var roles = this.roleManager
                .Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToList();

            return View(new AdminUsersVM
            {
                Users = users,
                Roles = roles
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(AddUserToFM model)
        {
            var roleExist = await this.roleManager.RoleExistsAsync(model.Role);
            var user = await this.userManager.FindByIdAsync(model.UserId);

            if (!roleExist || user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid identity details.");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            TempData.AddSuccessMessage($"User {user.UserName} success added to {model.Role} role.");
            await this.userManager.AddToRoleAsync(user, model.Role);

            return RedirectToAction(nameof(Index));
        }
    }
}