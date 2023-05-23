using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Softhouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(token);
                // Use the payload to retrieve user information or perform additional validation
                // Example: var userId = payload.Subject;

                // Assuming validation is successful, return an appropriate response
                return Ok(new { message = "Token validation successful" });
            }
            catch (InvalidJwtException)
            {
                // Token validation failed
                return Unauthorized(new { message = "Invalid token" });
            }
        }
    }
}
