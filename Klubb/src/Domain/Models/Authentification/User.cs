using Klubb.src.Domain.Models.Messages;
using Klubb.src.Domain.Models.Profil;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Klubb.src.Domain.Models.Authentification
{
    public class User : IdentityUser
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }

        public UserProfil UserProfil { get; set; }

    }
}
