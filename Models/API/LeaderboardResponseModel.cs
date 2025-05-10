using GameBackEnd.Models.Entities;

namespace GameBackEnd.Models.API
{
    public class LeaderboardResponseModel
    {
        public List<LeaderboardItemModel> Items { get; set; } = new List<LeaderboardItemModel>();
    }
}
