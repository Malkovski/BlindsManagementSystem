namespace Blinds.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using Common;

    public class LoginViewModel : MenuModel
    {
        [Required]
        [Display(Name = GlobalConstants.SignInNameText)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = GlobalConstants.Password)]
        public string Password { get; set; }

        [Display(Name = GlobalConstants.RememberMeText)]
        public bool RememberMe { get; set; }
    }
}