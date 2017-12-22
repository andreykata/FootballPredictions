namespace FootballAnalyzes.Services.Admin.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.Services.Admin.Models;

    public class AdminUserService : IAdminUserService
    {
        private readonly FootballAnalyzesDbContext db;

        public AdminUserService(FootballAnalyzesDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<AdminUserListingSM> All()
        {
            return this.db
               .Users
               .ProjectTo<AdminUserListingSM>()
               .ToList();
        }
    }
}
