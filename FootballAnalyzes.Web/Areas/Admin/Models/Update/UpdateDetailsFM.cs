namespace FootballAnalyzes.Web.Areas.Admin.Models.Update
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UpdateDetailsFM : IValidatableObject
    {
        public string DatesInfo { get; set; }
        [DataType(DataType.Date)]
        public DateTime NextGamesDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.NextGamesDate <= DateTime.UtcNow.AddDays(-1))
            {
                yield return new ValidationResult("Start date should be today or in the future.");
            }
        }
    }
}
