using Klubb.src.Domain.IServices;
using Klubb.src.Domain.Models.Authentification;
using Klubb.src.Domain.Models.Profil;
using Klubb.src.Infrastructure.Dbontext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Klubb.src.Infrastructure.Repositories
{
    public class UserProfilRepository : IUserProfilRepository
    {
        private readonly DataContext _dataContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserProfilRepository(DataContext dataContext, IPasswordHasher<User> passwordHasher)
        {
            _dataContext = dataContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> DeleteProfil(int profilId, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.
                Include(u => u.UserProfil).
                SingleOrDefaultAsync(u => u.UserId == profilId, cancellationToken);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"This {user.Email} not found" });
            }

            _dataContext.Remove(user);
            _dataContext.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateProfil(UserProfil updateProfil, CancellationToken cancellationToken)
        {
            var profil = await _dataContext.Users.
                Include(u => u.UserProfil).
                FirstOrDefaultAsync(u => u.Email == updateProfil.Email, cancellationToken);
            if (profil == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"This {profil.Email} not found" });
            }

            profil.UserName = updateProfil.UserName;
            profil.LastName = updateProfil.LastName;
            profil.FirstName = updateProfil.FirstName;
            profil.DateOfBirth = profil.DateOfBirth;

            _dataContext.Add(profil);
            _dataContext.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateUserPassword(string email, string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            var existingUser = await _dataContext.Users.
                                Include(u => u.UserProfil).
                                SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
            if (existingUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"This {email} not found" });
            }

            var verifiedPassword = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, oldPassword);
            if (verifiedPassword == PasswordVerificationResult.Failed)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"This password is incorrect" });
            }
            else
            {
                existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, newPassword);
            }
            _dataContext.Users.Update(existingUser);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

    }
}
