using LifeTrack.Core.Interfaces.Services.Security;
using LifeTrack.Core.Models.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = LifeTrack.Core.Models.Contracts.Auth.LoginRequest;

namespace LifeTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentificationService _authenticationService;

        public AuthController(IAuthentificationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            try
            {
                var loginResult = await _authenticationService.Login(request.Username, request.Password, ct);
                if (!loginResult.IsSuccess) return BadRequest($"Error during login: {loginResult.ErrorMessage}");

                HttpContext.Response.Cookies.Append("jwt", loginResult.Data!, new CookieOptions()
                {
                    Secure = false,
                    Expires = DateTime.UtcNow.AddHours(72),
                    SameSite = SameSiteMode.Lax
                });
                
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request, CancellationToken ct)
        {
            try
            {
                var registrationResult = await _authenticationService.Registration(request.Username, request.Password, request.Email, ct);
                if (!registrationResult.IsSuccess) return BadRequest($"Error during registration: {registrationResult.ErrorMessage}");
                return Created();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
        
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Response.Cookies.Delete("jwt");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
    }
}
