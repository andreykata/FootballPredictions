namespace FootballAnalyzes.Web.Models
{
    using System;

    using static FootballAnalyzes.Services.ServiceConstants;

    public class Pagination
    {
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)this.TotalCount / GamesPageSize);
        public int CurrentPage { get; set; }
        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;
        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
