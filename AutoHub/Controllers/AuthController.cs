using AutoHub.BizLogic.Auth;
using AutoHub.Models.RESTAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<LoginResponseModel> Login([FromBody] LoginModel loginModel)
        {
            // TODO: Implement login logic
            string token = _authService.GenerateToken(loginModel.Email);
            return Ok(new { accessToken = token });
        }
    }
}
