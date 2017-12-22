namespace FootballAnalyzes.Services.Admin
{
    using System.Collections.Generic;
    using FootballAnalyzes.Services.Admin.Models;

    public interface IAdminUserService
    {
        IEnumerable<AdminUserListingSM> All();
    }
}
