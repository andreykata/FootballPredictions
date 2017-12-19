using System.ComponentModel.DataAnnotations;

namespace FootballAnalyzes.Services.Models.Games
{
    public class GameStatisticSM
    {
        [Required]
        [Range(0, 100, ErrorMessage = "Corners must be between 0 and 50")]
        public int HomeTeamCorners { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Corners must be between 0 and 50")]
        public int AwayTeamCorners { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Shots must be between 0 and 50")]
        public int HomeTeamShotsOnTarget { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Shots must be between 0 and 50")]
        public int AwayTeamShotsOnTarget { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Shots must be between 0 and 50")]
        public int HomeTeamShotsWide { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Shots must be between 0 and 50")]
        public int AwayTeamShotsWide { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Fouls must be between 0 and 50")]
        public int HomeTeamFouls { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Fouls must be between 0 and 50")]
        public int AwayTeamFouls { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Offsides must be between 0 and 50")]
        public int HomeTeamOffsides { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Offsides must be between 0 and 50")]
        public int AwayTeamOffsides { get; set; }

        public override string ToString()
        {
            return $"{this.HomeTeamCorners.ToString().PadLeft(3, ' ')},{this.AwayTeamCorners.ToString().PadLeft(2, ' ')}," +
                $"{this.HomeTeamShotsOnTarget.ToString().PadLeft(2, ' ')},{this.AwayTeamShotsOnTarget.ToString().PadLeft(2, ' ')}," +
                $"{this.HomeTeamShotsWide.ToString().PadLeft(2, ' ')},{this.AwayTeamShotsWide.ToString().PadLeft(2, ' ')}," +
                $"{this.HomeTeamFouls.ToString().PadLeft(2, ' ')},{this.AwayTeamFouls.ToString().PadLeft(2, ' ')}," +
                $"{this.HomeTeamOffsides.ToString().PadLeft(2, ' ')},{this.AwayTeamOffsides.ToString().PadLeft(2, ' ')}";
        }
    }
}
