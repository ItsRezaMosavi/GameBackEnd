using GameBackEnd.Models;
using GameBackEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly RecordService _recordService;
        public RecordController(RecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpPost("SubmitScore")]
        public async Task<IActionResult> SubmitScoreAsync([FromBody] SubmitScoreRequest ScoreRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
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
                var result = await _recordService.GetRankedLeaderboardAsync();
                if(result == null)
                    return Problem(title: "server Error", statusCode: 500);
                return Ok(result);
        }

        [HttpGet("GetUserRank")]
        public async Task<IActionResult> GetUserRank([FromBody] User user)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            LeaderboardItem? userrank = await _recordService.GetUserRankAsync(user);
            return Ok(userrank);
        }
    }
}
