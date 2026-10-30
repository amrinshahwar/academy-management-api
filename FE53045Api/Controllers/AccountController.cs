using AcademyManagementApi.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AcademyManagementApi.Data.Entities;
using AcademyManagementApi.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcademyManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(AcademyDbContext _context) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _context.UserInfos.FirstOrDefaultAsync(u => u.UserName == model.UserName);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var hasher = new PasswordHasher<UserInfo>();
            var result = hasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.Password = hasher.HashPassword(user, model.Password);
                await _context.SaveChangesAsync();
            }

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
            claims.Add(new Claim(ClaimTypes.Name, user.FullName));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            var identity = new ClaimsIdentity(claims, "MyApi");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyApi", principal);
            return Ok(new
            {
                message = "Login successful.",
                userName = user.UserName,
                fullName = user.FullName,
                role = user.Role
            });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyApi");
            return Ok(new { message = "Logout successful." });
        }
    }
}




