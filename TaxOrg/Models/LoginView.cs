using System.ComponentModel.DataAnnotations;

namespace TaxOrg.Models
{
    public class LoginView
    {
        [Required(ErrorMessage = "������� �����")]
        public string Login { get; set; }

        [Required(ErrorMessage = "������� ������")]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}