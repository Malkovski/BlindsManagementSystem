using System.ComponentModel.DataAnnotations;

namespace Blinds.Web.Models
{
    public class LoginViewModel : MenuModel
    {
        [Required]
        [Display(Name = "Потребител")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [Display(Name = "Запомни ме?")]
        public bool RememberMe { get; set; }
    }
}