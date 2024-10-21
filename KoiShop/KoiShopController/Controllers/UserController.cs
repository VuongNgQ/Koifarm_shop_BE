using BusinessObject;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserService service, IConfiguration configuration)
        {
            _userService = service;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> GetUser(int page = 1, int pageSize = 10,
            string search = "", string sort = "")
        {
            var result = await _userService.GetAllUser(page, pageSize, search, sort);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        [HttpGet("profile")]
        [Authorize(Roles = "Admin, Manager, Staff, Customer")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var userProfile = await _userService.GetUserProfile(userId);

                return Ok(new ServiceResponseFormat<ResponseUserDTO>
                {
                    Success = true,
                    Data = userProfile,
                    Message = "User profile fetched successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponseFormat<ResponseUserDTO>
                {
                    Success = false,
                    Message = $"Failed to fetch user profile: {ex.Message}"
                });
            }
        }

        [HttpPost("createAccount")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(CreateUserDTO newUser)
        {
            var result = await _userService.CreateUser(newUser);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        [HttpPost("createStaff")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateStaff(CreateUserDTO newStaff)
        {
            var result = await _userService.CreateStaff(newStaff);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("createManager")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateManager(CreateUserDTO newManager)
        {
            var result = await _userService.CreateManager(newManager);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var loginResult = await _userService.LoginUser(loginRequest.Email, loginRequest.Password);

            if (!loginResult.Success)
            {
                return BadRequest(new { Message = loginResult.Message });
            }

            var user = loginResult.Data;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.RoleName)
            };

            var token = CreateToken(claims);

            return Ok(new
            {
                Token = token,
                UserName = user.Name,
                Role = user.RoleName,
                Message = loginResult.Message
            });
        }


        [HttpPut("updateUser/{userId}")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult>UpdateUser(int userId, UpdateUserDTO userDTO)
        {
            var result = await _userService.UpdateUser(userId, userDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] RequestPasswordResetDTO request)
        {
            var result = await _userService.ForgotPassword(request);
            if (result == null)
            {
                return BadRequest("Invalid email address.");
            }

            return Ok("Password reset email has been sent.");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            var result = await _userService.ResetPassword(resetPasswordDto);
            if (result == null)
            {
                return BadRequest("Invalid or expired token.");
            }

            return Ok("Password has been reset successfully.");
        }

        [HttpPut("profile")]
        [Authorize(Roles = "Admin, Manager, Staff, Customer")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO updateProfileDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _userService.UpdateProfile(userId, updateProfileDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPut("deleteUser/{userId}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUser(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPut("restoreUser/{userId}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> RestoreUser(int userId)
        {
            var result = await _userService.RestoreUser(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpDelete("deleteHard/{userId}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            var result = await _userService.RemoveUser(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        private string CreateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetSection("Jwt:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                                   issuer: _configuration["Jwt:Issuer"],
                                   audience: _configuration["Jwt:Audience"],
                                   claims: claims,
                                   expires: DateTime.UtcNow.AddHours(3),
                                   signingCredentials: cred
             );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
