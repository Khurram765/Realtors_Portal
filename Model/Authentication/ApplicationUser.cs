using Microsoft.AspNetCore.Identity;

namespace Realtors_Portal.Model.Authentication
{
    public class ApplicationUser:IdentityUser
    {
        public string UserContact { get; set; }
        public string UserAddress { get; set; }
        public string IsActive { get; set; }
        public string IsDeleted { get; set; }
    }
}
