using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameBackEnd.Models
{
    public class Record
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int Score { get; set; } = 0;
        [Required]
        public DateTime DateTime { get; set; }

        public User user { get; set; }
    }
}
