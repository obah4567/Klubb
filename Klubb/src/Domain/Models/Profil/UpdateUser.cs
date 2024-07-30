using System.ComponentModel.DataAnnotations;

namespace Klubb.src.Domain.Models.Profil
{
    public class UpdateUser
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

    }
}
