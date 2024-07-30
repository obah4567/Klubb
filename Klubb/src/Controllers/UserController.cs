using Klubb.src.Domain.DTO.AuthentificationDto;
using Klubb.src.Domain.DTO.UserProfilDto;
using Klubb.src.Domain.Services;
using Klubb.src.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Klubb.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userService;
        private readonly TokenService _tokenService;

        public UserController(IUserRepository userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            var register = await _userService.RegisterUserAsync(registerDto, cancellationToken);
            if (!register.Succeeded)
            {
                return BadRequest(register.Errors);
            }
            return Ok(register);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            var login = await _userService.LoginUserAsync(loginDto, cancellationToken);
            if (! login.Succeeded) 
            {
                return Unauthorized("Your access denied");
            }

            var user = await _userService.GetUserByEmailAsync(loginDto.Email, cancellationToken);
            var token = _tokenService.GenerateToken(user.UserId);

            return Ok(new {token});
        }

        //Faire un role Admin pour etre le seul à voir cette route
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();  
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
        {
            var update = await _userService.UpdateUserAsync(id, updateUserDTO, cancellationToken);
            if (! update.Succeeded)
            {
                return BadRequest(update.Errors);
            }
            return Ok(update);
        }
    }
}
