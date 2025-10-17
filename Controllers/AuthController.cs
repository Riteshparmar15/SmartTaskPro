using Microsoft.AspNetCore.Mvc;
using SmartTaskPro.DTOs;
using SmartTaskPro.Services;

namespace SmartTaskPro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        {
        }
    }
}
