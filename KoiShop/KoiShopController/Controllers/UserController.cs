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
        /// <summary>
        /// Get a list of users with pagination.
        /// </summary>
        /// <param name="page">Current page.</param>
        /// <param name="pageSize">Number of users per page.</param>
        /// <param name="search">Search keyword.</param>
        /// <param name="sort">Sort criteria.</param>
        /// <returns>List of users with pagination.</returns>
        /// <response code="200">Returns a list of users.</response>
        /// <response code="404">No users found.</response>
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
        /// <summary>
        /// Get the user details.
        /// </summary>
        /// <returns>The user's information.</returns>
        /// <response code="200">Returns the user information.</response>
        /// <response code="404">No user with the given ID was found.</response>
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
        /// <summary>
        /// Create a new user account.
        /// </summary>
        /// <param name="newUser">Details of the user to create.</param>
        /// <returns>Information of the user just created.</returns>
        /// <response code="200">Returns the user that was successfully created.</response>
        /// <response code="400">Invalid user creation request.</response>
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
        /// <summary>
        /// Create an account for an staff.
        /// </summary>
        /// <param name="newStaff">Details of the staff to create.</param>
        /// <returns>Information of the staff just created.</returns>
        /// <response code="200">Returns the staff just created successfully.</response>
        /// <response code="400">Invalid staff creation request.</response>
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
        /// <summary>
        /// Create an account for a manager.
        /// </summary>
        /// <param name="newManager">Details of the manager to create.</param>
        /// <returns>Information of the manager just created.</returns>
        /// <response code="200">Returns the manager just created successfully.</response>
        /// <response code="400">Invalid request to create manager.</response>
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
        /// <summary>
        /// Log in to the user account.
        /// </summary>
        /// <param name="loginRequest">Credentials of the user.</param>
        /// <returns>The user's authentication information.</returns>
        /// <response code="200">Returns the user's authentication information.</response>
        /// <response code="401">The email or password is incorrect.</response>
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
                UserCartId = user.UserCartId, 
                Message = loginResult.Message
            });
        }

        /// <summary>
        /// Update user information.
        /// </summary>
        /// <param name="userId">ID of the user to update.</param>
        /// <param name="userDTO">New user information.</param>
        /// <returns>User information after update.</returns>
        /// <response code="200">Returns the user that was successfully updated.</response>
        /// <response code="404">No user with the given ID was found.</response>
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
        /// <summary>
        /// Password reset request (forgot password).
        /// </summary>
        /// <param name="request">Password reset request information, including user email.</param>
        /// <returns>Password reset request confirmation message.</returns>
        /// <response code="200">Password reset request successful.</response>
        /// <response code="404">No user with the provided email found.</response>
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
        /// <summary>
        /// Reset the user's password based on the token sent via email.
        /// </summary>
        /// <param name="resetPasswordDto">Password reset information, including confirmation token and new password.</param>
        /// <returns>Password reset confirmation message.</returns>
        /// <response code="200">Password reset successful.</response>
        /// <response code="400">Invalid token or invalid password reset request.</response>
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
        /// <summary>
        /// Update the user's profile.
        /// </summary>
        /// <param name="updateProfileDTO">The profile information to update.</param>
        /// <returns>The profile information after updating.</returns>
        /// <response code="200">Returns the profile that was successfully updated.</response>
        /// <response code="404">No user with the given ID was found.</response>
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
        /// <summary>
        /// Mark the user as removed.
        /// </summary>
        /// <param name="userId">The ID of the user to be removed.</param>
        /// <returns>Confirm the user deletion.</returns>
        /// <response code="200">Returns confirmation that the user has been deleted.</response>
        /// <response code="404">No user with the given ID was found.</response>
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
        /// <summary>
        /// Restore a deleted user account.
        /// </summary>
        /// <param name="userId">ID of the user to restore.</param>
        /// <returns>Confirm user account restoration.</returns>
        /// <response code="200">User restored successfully.</response>
        /// <response code="404">No user found with the given ID.</response>
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
