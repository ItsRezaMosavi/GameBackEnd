using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameBackEnd.Models.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage ="Username is required!")]
        [StringLength(20, ErrorMessage = "User Name length must be between 3 to 20", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Email is required!")]
        [EmailAddress(ErrorMessage = "Email format is not valid!")]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required!")]
        public string PasswordHash { get; set; }

        public ICollection<Record>? records { get; set; }
    }
}
