using System.ComponentModel.DataAnnotations;

namespace Realtors_Portal.Model.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string UserPassword { get; set; }
        [Required(ErrorMessage ="User Contact is required")]
        public string UserContact { get; set; }
        [Required(ErrorMessage = "User Address is required")]
        public string UserAddress { get; set; }
        [Required]
        public string IsActive { get; set; }
        [Required]
        public string IsDeleted { get; set; }
    }
}
