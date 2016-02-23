namespace Blinds.Web.Models
{
    using Blinds.Common;
    using System.ComponentModel.DataAnnotations;

    public class ManageUserViewModel : MenuModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.CurrentPassword)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.UserPasswordLengthErrorMessage, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.NewPassword)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.UserRepeatNewPassword)]
        [Compare("NewPassword", ErrorMessage = GlobalConstants.UserPasswordNotmatchErrorMessage)]
        public string ConfirmPassword { get; set; }
    }
}