using Klubb.src.Domain.IServices;
using Klubb.src.Domain.Models.Authentification;
using Klubb.src.Domain.Models.Profil;
using Klubb.src.Infrastructure.Dbontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Klubb.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilController : ControllerBase
    {
        private readonly IUserProfilRepository _userProfilRepository;
        private readonly DataContext _dataContext;

        public ProfilController(IUserProfilRepository userProfilRepository, DataContext dataContext)
        {
            _userProfilRepository = userProfilRepository;
            _dataContext = dataContext;
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdateUserPassword(string email, string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            var udpatePwd = await _userProfilRepository.UpdateUserPassword(email, oldPassword, newPassword, cancellationToken);
            if (!udpatePwd.Succeeded)
            {
                return BadRequest(udpatePwd.Errors);
            }
            return Ok(udpatePwd);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfil(int id, CancellationToken cancellationToken)
        {
            var delete = await _userProfilRepository.DeleteProfil(id, cancellationToken);
            if (!delete.Succeeded)
            {
                return BadRequest(delete.Errors);
            }
            return Ok(delete);
        }

        [HttpGet("GetAll-Profil")]
        public async Task<IActionResult> GetAllProfil()
        {
            var list = await _dataContext.Users.ToListAsync();
            return Ok(list);
        }

        [HttpPut("update-profil")]
        public async Task<IActionResult> UpdateUserProfil([FromBody] UserProfil userProfil, CancellationToken cancellationToken)
        {
            var profil = await _userProfilRepository.UpdateProfil(userProfil, cancellationToken);
            if (! profil.Succeeded)
            {
                return BadRequest(profil.Errors);
            }
            return Ok(profil);
        }

        [HttpPost("upload-profile")]
        public async Task<IActionResult> UploadProfilPicture([FromForm] IFormFile profilPicture, string email, CancellationToken cancellationToken)
        {
            var existingUser = await _dataContext.Users
                                                .Include(u => u.UserProfil)
                                                .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
            if (existingUser == null)
            {
                return NotFound($"User with email '{email}' not found.");
            }
            if (profilPicture == null || profilPicture.Length == 0)
            {
                return BadRequest("No file uploaded or file is empty.");
            }
            // Directory to save profile pictures
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profil_pictures");

            // Ensure directory exists, create if not
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            // Generate unique file name for profile picture (you can adjust this as per your needs)
            var fileName = $"{existingUser.UserId}_{Guid.NewGuid().ToString()}_{Path.GetFileName(profilPicture.FileName)}";
            var filePath = Path.Combine(uploadDirectory, fileName);
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilPicture.CopyToAsync(stream, cancellationToken);
                }

                // Update user's profile picture path in database
                //existingUser.ProfilPicture = filePath;
                _dataContext.Users.Update(existingUser);
                await _dataContext.SaveChangesAsync(cancellationToken);

                return Ok(new { Path = filePath });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while uploading the profile picture: {ex.Message}");
            }
        }



    }
}
