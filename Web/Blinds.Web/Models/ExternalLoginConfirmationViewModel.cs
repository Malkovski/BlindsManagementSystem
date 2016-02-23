namespace Blinds.Web.Models
{
    using Blinds.Common;
    using System.ComponentModel.DataAnnotations;

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = GlobalConstants.UserNameText)]
        public string UserName { get; set; }
    }
}