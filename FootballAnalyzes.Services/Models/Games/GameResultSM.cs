namespace FootballAnalyzes.Services.Models.Games
{
    using System.ComponentModel.DataAnnotations;
    using FootballAnalyzes.Data.Models;

    public class GameResultSM
    {
        public ResultEnum Result { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "Goals must be between 0 and 50")]
        public int HomeTeamGoals { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "Goals must be between 0 and 50")]
        public int AwayTeamGoals { get; set; }

        public override string ToString()
        {
            return $"{this.Result.ToString()},{this.HomeTeamGoals}:{this.AwayTeamGoals}";
        }
    }
}
