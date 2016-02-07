using System.ComponentModel.DataAnnotations;

namespace Blinds.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Потребителско име")]
        public string UserName { get; set; }
    }
}