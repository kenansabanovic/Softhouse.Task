using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Softhouse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("validate")]
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
                return Ok(new { message = "Token validation successful" });
            }
            catch (InvalidJwtException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }
    }
}