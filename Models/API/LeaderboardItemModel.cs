namespace GameBackEnd.Models.API
{
    public class LeaderboardItemModel
    {
        public int Rank { get; set; }
        public string UserName { get; set; }
        public int BestScore { get; set; }
    }
}
