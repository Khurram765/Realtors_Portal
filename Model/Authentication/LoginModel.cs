using System.ComponentModel.DataAnnotations;

namespace Realtors_Portal.Model.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Email is required")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string UserPassword { get; set; }
    }
}
