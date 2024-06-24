using System.ComponentModel.DataAnnotations;

namespace SBWV.Models
{
    public class Login
    {
        [Display(Name = "Почта")]
        public string Email { get; set; } = "";
        [Display(Name = "Пароль")]
        public string Password { get; set; } = "";
    }
}
