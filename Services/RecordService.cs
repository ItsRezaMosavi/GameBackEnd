using GameBackEnd.Data;
using GameBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace GameBackEnd.Services
{
    public class RecordService
    {
        private readonly GameDbContext _context;
        private readonly UserService _userService;
        public RecordService(GameDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<ServiceResult> SubmitRecordAsync(SubmitScoreRequest ScoreRequest)
        {
            // check the user authentication
            User? user = await _userService.GetUserByUserNameAsync(ScoreRequest.UserName);
            if (user == null)
                throw new Exception("user can not be null");

            try
            {
                Record record = new Record()
                {
                    UserId = user.Id,
                    Score = ScoreRequest.Score,
                    DateTime = DateTime.Now,
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
        public async Task<LeaderboardItem?> GetUserRankAsync(User user)
        {
            var ranked = await GetRankedLeaderboardAsync();
            return ranked.Where(r => r.UserName == user.UserName).FirstOrDefault();
        }
        public async Task<List<LeaderboardItem>?> GetRankedLeaderboardAsync()
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
                          UserName = u.UserName,
                          BestScore = s.bestScore
                      }).ToListAsync();

                int rank = 0;
                int? previousScore = null;
                List<LeaderboardItem> ranked = new List<LeaderboardItem>();
                foreach (var item in Items)
                {
                    if (previousScore == null || item.BestScore < previousScore)
                        rank++;
                    ranked.Add(new LeaderboardItem
                    {
                        Rank = rank,
                        UserName = item.UserName,
                        BestScore = item.BestScore
                    });
                    previousScore = item.BestScore;
                }
                return ranked;
            }
            catch
            {
                return null;
            }
        }
    }
}
