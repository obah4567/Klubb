using Klubb.src.Domain.DTO.AuthentificationDto;
using Klubb.src.Domain.DTO.UserProfilDto;
using Klubb.src.Domain.Models.Authentification;
using Klubb.src.Domain.Services;
using Klubb.src.Infrastructure.Dbontext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Klubb.src.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRepository(DataContext dataContext, IPasswordHasher<User> passwordHasher)
        {
            _dataContext = dataContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            var existEmail = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email, cancellationToken);
            if (existEmail != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"This {existEmail.Email} was already exist" });
            }

            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Username,
                FirstName = registerDto.Firstname,
                LastName = registerDto.Lastname,
                DateOfBirth = registerDto.DateOfBirth
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public Task<User> Disconnected(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<SignInResult> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var existingUser = await _dataContext.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email, cancellationToken: cancellationToken);
            if (existingUser == null)
            {
                return SignInResult.Failed;
            }

            var result = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return SignInResult.Failed;
            }

            return SignInResult.Success;
        }

        public async Task<IdentityResult> UpdateUserAsync(int id, UpdateUserDTO updateUserDto, CancellationToken cancellationToken)
        {
            var existingUser = await _dataContext.Users.SingleOrDefaultAsync(u => u.UserId == id, cancellationToken);
            if (existingUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"This {existingUser.Email} not found" });
            }

            existingUser.FirstName = updateUserDto.FirstName;
            existingUser.LastName = updateUserDto.LastName;

            _dataContext.Users.Add(existingUser);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dataContext.Users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        }


        /*public async Task<IdentityResult> UpdateUserPassword(string email, string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            var existingUser = await _dataContext.Users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
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
        }*/

    }
}
