﻿namespace FootballAnalyzes.Web.Models.AccountViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static FootballAnalyzes.Data.DataConstants;

    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [MinLength(UserNameMinLength)]
        [MaxLength(UserNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
    }
}
