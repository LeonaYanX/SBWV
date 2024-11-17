using System.ComponentModel.DataAnnotations;

namespace SBWV.Models.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Почта")]
        public string Email { get; set; } = "";
        [Display(Name = "Пароль")]
        public string Password { get; set; } = "";
    }
}
