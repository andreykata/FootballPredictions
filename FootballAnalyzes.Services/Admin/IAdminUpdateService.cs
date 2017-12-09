using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalyzes.Services.Admin
{
    public interface IAdminUpdateService
    {
        void UpdateDb(DateTime nextGamesDate);
    }
}
