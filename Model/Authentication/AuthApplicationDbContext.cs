using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Realtors_Portal.Model.Authentication
{
    public class AuthApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public AuthApplicationDbContext(DbContextOptions<AuthApplicationDbContext> options) : base(options) { }
    }
}
