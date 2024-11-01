using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;
using OpenTelemetry.Trace;
using QlySach_API.Model.Auth;
using QlySach_API.Service;


namespace QlySach_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AuthService authService;
        public LoginController(AuthService authService) 
        {
            this.authService = authService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest();
            }

            try
            {
                var response = await authService.AuthenticateAsync(request.UserName, request.Password);
                if (response == null || string.IsNullOrEmpty(response.jwtToken))
                {
                    return BadRequest();
                }
                return Ok(new { Token = response.jwtToken });
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
