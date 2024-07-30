using Klubb.src.Domain.DTO.AuthentificationDto;
using Klubb.src.Domain.DTO.UserProfilDto;
using Klubb.src.Domain.Models.Authentification;
using Microsoft.AspNetCore.Identity;

namespace Klubb.src.Domain.Services
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUserAsync (RegisterDto registerDto, CancellationToken cancellationToken);

        Task<SignInResult> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken);

        Task<User> GetUserByIdAsync (int id);

        Task<IdentityResult> UpdateUserAsync(int id, UpdateUserDTO updateUserDto, CancellationToken cancelToken);

        Task<User> Disconnected (int id);
        Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

        /*        Task<IdentityResult> UpdateUserPassword(string email, string oldPassword, string newPassword, CancellationToken cancellationToken);
        */
    }
}
