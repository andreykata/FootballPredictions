namespace FootballAnalyzes.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    using static FootballAnalyzes.Data.DataConstants;
    
    public class User : IdentityUser
    {
        [Required]
        [MinLength(UserNameMinLength)]
        [MaxLength(UserNameMaxLength)]
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }
    }
}
