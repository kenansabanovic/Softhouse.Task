using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Softhouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        [HttpPost]
        [Route("validate")]
        [EnableCors("MyCorsPolicy")]
        public async Task<IActionResult> ValidateToken()
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var jwtToken = authorizationHeader.Substring("Bearer ".Length);

                var payload = await GoogleJsonWebSignature.ValidateAsync(jwtToken);
                var redirectUrl = GoogleCallback();
                var properties = new AuthenticationProperties
                {
                    RedirectUri = "",
                    Items =
            {
                { "scheme", "Google" }
            }
                };
                return Challenge(properties, "Google");
                // Use the payload to retrieve user information or perform additional validation
                // Example: var userId = payload.Subject;

                // Assuming validation is successful, return an appropriate response
                //return Ok(new { message = "Token validation successful" });
            }
            catch (InvalidJwtException)
            {
                // Token validation failed
                return Unauthorized(new { message = "Invalid token" });
            }

            
        }
        [HttpGet("GoogleCallback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authResult = await HttpContext.AuthenticateAsync();

            if (!authResult.Succeeded)
            {
                return BadRequest();
            }

            var emailClaim = authResult.Principal.FindFirst(ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(emailClaim.Value);

            if (user == null)
            {
                return BadRequest();
            }

            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpirationHours"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public class ApplicationUser : IdentityUser
        {
            // Add additional properties and customizations as needed
        }
    }
}
