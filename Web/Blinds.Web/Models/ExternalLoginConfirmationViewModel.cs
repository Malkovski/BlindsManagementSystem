using System.ComponentModel.DataAnnotations;

namespace Blinds.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Потребител")]
        public string UserName { get; set; }
    }
}