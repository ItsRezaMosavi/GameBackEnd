using GameBackEnd.Models;
using GameBackEnd.Models.API;
using GameBackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                ServiceResult result = await _authService.RegisterAsync(model);
                if (result.IsSuccess)
                    return Ok(result.Message);
                else
                    return Problem(title: result.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _authService.LoginAsync(model);
            if(!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
            //////////////////////////////////////////////////
        }
    }
}
