namespace FootballAnalyzes.Services.Admin.Implementations
{
    using System;
    using FootballAnalyzes.Data;
    using FootballAnalyzes.UpdateDatabase;

    public class AdminUpdateService : IAdminUpdateService
    {
        private readonly FootballAnalyzesDbContext db;

        public AdminUpdateService(FootballAnalyzesDbContext db)
        {
            this.db = db;
        }

        public void UpdateDb(DateTime nextGamesDate)
        {
            var updateDb = new StartUpdate(this.db, nextGamesDate);
        }
    }
}
