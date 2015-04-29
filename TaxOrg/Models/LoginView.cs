using System.ComponentModel.DataAnnotations;

namespace TaxOrg.Models
{
    public class LoginView
    {
        [Required(ErrorMessage = "¬ведите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "¬ведите пароль")]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}