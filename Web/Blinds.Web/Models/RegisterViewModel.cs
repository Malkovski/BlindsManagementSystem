namespace Blinds.Web.Models
{
    using Blinds.Common;
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel : MenuModel
    {
        [Required]
        [Display(Name = GlobalConstants.SignInNameText)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.UserPasswordLengthErrorMessage, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.UserRepeatPassword)]
        [Compare("Password", ErrorMessage = GlobalConstants.UserPasswordNotmatchErrorMessage)]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = GlobalConstants.UserNameText)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = GlobalConstants.UserLastNameText)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = GlobalConstants.UserAddressText)]
        public string Address { get; set; }
    }
}