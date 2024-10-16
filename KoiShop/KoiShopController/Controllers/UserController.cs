using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Service;
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

        [HttpPost]
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

        [HttpPost("CreateStaff")]
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

        [HttpPost("CreateManager")]
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

        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> GetUser(int page=1, int pageSize=10,
            string search="", string sort="")
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
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var result = await _userService.LoginUser(loginRequest.Email, loginRequest.Password);
            if (!result.Success)
            {
                return BadRequest("Your email or password is incorrect!");
            }
            var user = result.Data;
            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.Name),
               new Claim(ClaimTypes.NameIdentifier, user.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            // Tạo token
            var token = CreateToken(claims);
            return Ok(new { Token = token, Role = user.Role.RoleName });
        }


        //[HttpGet("Login/Manager/{email}, {pass}")]
        //public async Task<IActionResult> LoginAdmin(string email, string pass)
        //{
        //    var result = await _userService.LoginAdmin(email, pass);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return NotFound(result.Message);
        //    }
        //}
        //[HttpGet("Login/Customer/{email}, {pass}")]
        //public async Task<IActionResult> LoginCustomer(string email, string pass)
        //{
        //    var result = await _userService.LoginCustomer(email, pass);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return NotFound(result.Message);
        //    }
        //}
        //[HttpGet("Login/Staff/{email}, {pass}")]
        //public async Task<IActionResult> LoginStaff(string email, string pass)
        //{
        //    var result = await _userService.LoginStaff(email, pass);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return NotFound(result.Message);
        //    }
        //}

        [HttpPut("updateUser/{userId}")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult>UpdateUser(int userId, ResponseUserDTO userDTO)
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
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            var result=await _userService.RemoveUser(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
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

        [HttpPut("updateProfile/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateProfileDTO updateProfileDTO)
        {
            var result = await _userService.UpdateProfile(userId, updateProfileDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
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
                                   expires: DateTime.UtcNow.AddHours(1),
                                   signingCredentials: cred
             );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        [HttpPost("request-password-reset")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDTO request)
        {
            var result = await _userService.GeneratePasswordResetToken(request.Email);
            if (!result)
            {
                return BadRequest("Invalid email address.");
            }

            return Ok("Password reset email has been sent.");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            var result = await _userService.ResetPassword(resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (!result)
            {
                return BadRequest("Invalid or expired token.");
            }

            return Ok("Password has been reset successfully.");
        }
    }
}
