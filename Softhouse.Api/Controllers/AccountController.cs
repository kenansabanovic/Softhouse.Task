using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Softhouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly string _jwtSecretKey = "rshzXFtXqdYI9zn5mPZL8/fkzkh1lR1GaXCCDAdaYyY=";

        [HttpGet("login")]
        public IActionResult LoginWithGoogle()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleLoginCallback))
            };

            return Challenge(authenticationProperties, "Google");
        }

        [HttpGet("login/google-callback")]
        public async Task<IActionResult> GoogleLoginCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync();

            if (!authenticateResult.Succeeded)
            {
                // Handle authentication failure
                return Unauthorized();
            }

            var userEmail = authenticateResult.Principal.FindFirst(ClaimTypes.Email).Value;

            // Generate JWT token
            var token = GenerateJwtToken(userEmail);

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(string userEmail)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Email, userEmail)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}