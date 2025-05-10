using System.ComponentModel.DataAnnotations;

namespace GameBackEnd.Models.Entities
{
    public class LeaderboardItemModel
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Rank { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [Range(0,int.MaxValue)]
        public int BestScore { get; set; }
    }
}
