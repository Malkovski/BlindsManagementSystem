namespace Blinds.Web.Models
{
    using Blinds.Common;
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = GlobalConstants.UserNickText)]
        public string UserName { get; set; }
    }
}