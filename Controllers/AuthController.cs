using GameBackEnd.Models;
using GameBackEnd.Models.API;
using GameBackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

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


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<AuthResponseModel>> RegisterAsync([FromBody] RegisterRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _authService.RegisterAsync(request);
                if (result is null)
                    return Unauthorized();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseModel>> LoginAsync([FromBody] LoginRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.LoginAsync(request);
            if (result is null)
                return Unauthorized();
            return result;
        }
    }
}
