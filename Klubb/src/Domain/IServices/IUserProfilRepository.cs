using Klubb.src.Domain.DTO.AuthentificationDto;
using Klubb.src.Domain.Models.Profil;
using Microsoft.AspNetCore.Identity;

namespace Klubb.src.Domain.IServices
{
    public interface IUserProfilRepository
    {
        Task<IdentityResult> DeleteProfil(int profilId, CancellationToken cancellationToken);
        Task<IdentityResult> UpdateProfil(UserProfil updateProfil, CancellationToken cancellationToken);
        Task<IdentityResult> UpdateUserPassword(string email, string oldPassword, string newPassword, CancellationToken cancellationToken);
    }
}
