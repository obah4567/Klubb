namespace Klubb.src.Domain.DTO.UserProfilDto
{
    public class UpdateProfilDto
    {
        public string UserImageProfil { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public DateTime BirthOfDay { get; set; }
    }
}
