using GameBackEnd.Data;
using GameBackEnd.Models;
using GameBackEnd.Models.API;
using GameBackEnd.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace GameBackEnd.Services
{
    [Authorize]
    public class RecordService
    {
        private readonly GameDbContext _context;
        private readonly UserService _userService;
        public RecordService(GameDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<ServiceResult> SubmitRecordAsync(SubmitScoreRequestModel ScoreRequest)
        {
            
            User? user = await _userService.GetUserByUserNameAsync(ScoreRequest.UserName);
            if (user == null)
                throw new Exception("user can not be null");

            try
            {
                Record record = new Record()
                {
                    UserId = user.Id,
                    Score = ScoreRequest.Score,
                    user = user
                };

                await _context.Records.AddAsync(record);
                await _context.SaveChangesAsync();
                return ServiceResult.Success("Score Submitted Sucessfully", user.UserName);
            }
            catch
            {
                return ServiceResult.Failure("Server Error");
            }
        }
        //public async Task<LeaderboardItemModel?> GetUserRankAsync(User user)
        //{
        //    var ranked = await GetRankedLeaderboardAsync();
        //    return ranked.Items.Where(r => r.UserName == user.UserName).FirstOrDefault();
        //}
        public async Task<LeaderboardResponseModel> GetRankedLeaderboardAsync()
        {
            try
            {
                var Items = await _context.Records.GroupBy(r => r.UserId).Select(g => new
                {
                    UserId = g.Key,
                    bestScore = g.Max(g => g.Score)
                }).OrderByDescending(o => o.bestScore)
               .Join(
                      _context.Users,
                      s => s.UserId,
                      u => u.Id,
                      (s, u) => new
                      {
                          username = u.UserName,
                          BestScore = s.bestScore
                      }).ToListAsync();

                int rank = 0;
                int? previousScore = null;
                LeaderboardResponseModel result = new LeaderboardResponseModel();
                foreach (var item in Items)
                {
                    if (previousScore == null || item.BestScore < previousScore)
                        rank++;
                    result.Items.Add(new LeaderboardItemModel
                    {
                        Rank = rank,
                        UserName = item.username,
                        BestScore = item.BestScore
                    });
                    previousScore = item.BestScore;
                    if (rank == 10)
                        break;
                }
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
