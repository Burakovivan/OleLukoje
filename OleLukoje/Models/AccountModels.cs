using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace OleLukoje.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Column, Required]
        [StringLength(20, MinimumLength = 6)]
        [Display(Name = "User nameL")]
        public string UserName { get; set; }

        [Column, Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Column, Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number:")]
        public string Phone { get; set; }

        [Column]
        [Display(Name = "Your name:")]
        public string Name { get; set; }

        [Column]
        [Display(Name = "Yours surname:")]
        public string Surname { get; set; }

        [Column]
        [Display(Name = "Country:")]
        public string Country { get; set; }

        [Column]
        [Display(Name = "City:")]
        public string City { get; set; }

        [Column]
        [Display(Name = "Yours organization:")]
        public string Organization { get; set; }

        /// <summary>
        /// Связь один-ко-многим с юзером (у одного юзера много обьявлений)
        /// </summary>
        public virtual ICollection<Ad> Ads { get; set; }

        public UserProfile()
        {
            Ads = new List<Ad>();
        }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password:")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password:")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password:")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name:")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "Name must be at least 6 characters long and maximum - 20.", MinimumLength = 6)]
        [Display(Name = "User name:")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password:")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number:")]
        public string Phone { get; set; }

        [Display(Name = "Your name:")]
        public string Name { get; set; }

        [Display(Name = "Yours surname:")]
        public string Surname { get; set; }

        [Display(Name = "Country:")]
        public string Country { get; set; }

        [Display(Name = "City:")]
        public string City { get; set; }

        [Display(Name = "Yours organization:")]
        public string Organization { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
