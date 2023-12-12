using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Realtors_Portal.Model.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Realtors_Portal.Controllers.Authentication
{
    [Route("Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        public string storeRole;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.UserEmail);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var hashPassword = new PasswordHasher<ApplicationUser>();
            var hashedPassword = hashPassword.HashPassword(null, model.UserPassword);
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.UserEmail,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                PasswordHash = hashedPassword,
                UserContact = model.UserContact,
                UserAddress = model.UserAddress,
                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted
            };

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            foreach (var role in new[] { UserRoles.Admin, UserRoles.PrivateSeller, UserRoles.Agent })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

                if (await roleManager.RoleExistsAsync(role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }

            return Ok(new Response { Status = "Success", Message = "Admin created successfully!" });
        }

        [HttpPost]
        [Route("register-seller")]
        public async Task<IActionResult> RegisterPrivateSeller([FromBody] RegisterModel model)
        {
            return await RegisterUserWithRole(model, UserRoles.PrivateSeller);
        }

        [HttpPost]
        [Route("register-agent")]
        public async Task<IActionResult> RegisterAgent([FromBody] RegisterModel model)
        {
            return await RegisterUserWithRole(model, UserRoles.Agent);
        }

        private async Task<IActionResult> RegisterUserWithRole(RegisterModel model, string roleName)
        {
            var userExists = await userManager.FindByEmailAsync(model.UserEmail);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var hashPassword = new PasswordHasher<ApplicationUser>();
            var hashedPassword = hashPassword.HashPassword(null, model.UserPassword);
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.UserEmail,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                PasswordHash = hashedPassword,
                UserContact = model.UserContact,
                UserAddress = model.UserAddress,
                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted
            };

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));

            if (await roleManager.RoleExistsAsync(roleName))
            {
                await userManager.AddToRoleAsync(user, roleName);
            }

            return Ok(new Response { Status = "Success", Message = $"{roleName} created successfully!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.Users
            .Where(u => u.Email == model.UserEmail && u.IsActive == "Y" && u.IsDeleted == "N")
            .FirstOrDefaultAsync();
            if (user != null && await userManager.CheckPasswordAsync(user, model.UserPassword))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id), 
                    new Claim("UserEmail", user.Email),            
                    new Claim("UserContact", user.UserContact),         
                    new Claim("UserAddress", user.UserAddress)
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    storeRole = userRole;
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    role = storeRole,
                    userId = user.Id,        // Include user ID in the response
                    userEmail = user.Email,   // Include user email in the response
                    userContact = user.UserContact, // Include user contact in the response
                    userAddress = user.UserAddress
                });
            }
            return Ok("Invalid Email or Password");
        }


    }
}
