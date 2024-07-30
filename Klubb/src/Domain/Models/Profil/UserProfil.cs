using Klubb.src.Domain.Models.Authentification;
using Klubb.src.Domain.Models.Messages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Klubb.src.Domain.Models.Profil
{
    public class UserProfil
    {
        [Key]
        public int UserId { get; set; }
        public string ProfilPicture { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public DateTime BirthOfDay { get; set; }

        public User User { get; set; }

        public List<UserProfil> Friends { get; set; } = new List<UserProfil>();
        public List<Message> Messages { get; set; } = new List<Message>();
        //

    }
}
