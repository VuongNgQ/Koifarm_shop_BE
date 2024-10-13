using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
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
    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserService service, IConfiguration configuration)
        {
            _userService = service;
            _configuration = configuration;
        }

        [HttpPost]
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
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateStaff(CreateUserDTO newStaff)
        {
            newStaff.RoleId = 2;

            var result = await _userService.CreateUser(newStaff);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Staff")]
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
        [HttpGet("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequest)
        {
            var result = await _userService.LoginUser(loginRequest.Email, loginRequest.Password);
            if (result == null)
            {
                return BadRequest("Your email or password is incorrect!");
            }

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, result.Data.Name),
            new Claim(ClaimTypes.NameIdentifier, result.Data.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, result.Data.Role.RoleName)
            };

            // Tạo token
            var token = CreateToken(claims);
            return Ok(new { Token = token, Role = result.Data.Role.ToString() });
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult>UpdateUser(int id, ResponseUserDTO userDTO)
        {
            var result = await _userService.UpdateUser(id, userDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result=await _userService.DeleteUser(id);
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
                                   expires: DateTime.UtcNow.AddHours(1),
                                   signingCredentials: cred
             );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
