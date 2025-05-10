using GameBackEnd.Models;
using GameBackEnd.Models.Entities;
using GameBackEnd.Models.API;
using GameBackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace GameBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecordController : ControllerBase
    {
        private readonly RecordService _recordService;
        public RecordController(RecordService recordService)
        {
            _recordService = recordService;
        }
        [HttpPost("SubmitScore")]
        public async Task<IActionResult> SubmitScoreAsync([FromBody] SubmitScoreRequestModel ScoreRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var UserName = jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Name).Value;

                if (UserName != ScoreRequest.UserName)
                    return BadRequest();

                ServiceResult result = await _recordService.SubmitRecordAsync(ScoreRequest);
                if (result.IsSuccess)
                    return Ok(result);
                else
                    return Problem(title: result.Message, statusCode: 500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Leaderboard")]
        public async Task<IActionResult> GetLeaderboardAsync()
        {
            var response = await _recordService.GetRankedLeaderboardAsync();
            if (response == null)
                return Problem(title: "server Error", statusCode: 500);
            return Ok(response);
        }
        [Authorize]
        [HttpGet("GetUserRank")]
        public async Task<IActionResult> GetUserRank([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            LeaderboardItemModel? userrank = await _recordService.GetUserRankAsync(user);
            return Ok(userrank);
        }
    }
}
