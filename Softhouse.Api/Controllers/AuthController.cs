using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Softhouse.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("google")]
        public IActionResult GoogleLogin([FromBody] GoogleTokenRequest request)
        {
            // Validate the received `request.token` using the Google API or your preferred OAuth2 library
            // Example: using Google.Apis.Auth library
            var payload = GoogleJsonWebSignature.ValidateAsync(request.Token).Result;

            // If the token is valid, you can proceed with user authentication
            // Example: return a JWT token or create a session for the user
            //var user = YourAuthenticationLogic.GetUserByEmail(payload.Email);

            var token = request.Token;

            // Return an appropriate response based on your authentication logic
            // Example: return the generated token
             return Ok(new { token });
;
        }
    }

    public class GoogleTokenRequest
    {
        public string Token { get; set; }
    }
}