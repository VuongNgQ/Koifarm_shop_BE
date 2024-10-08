using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService service)
        {
            _userService = service;
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
        [HttpGet]
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
        public async Task<IActionResult> Login(string email, string pass)
        {
            var result = await _userService.LoginUser(email, pass);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpGet("Login/Manager/{email}, {pass}")]
        public async Task<IActionResult> LoginAdmin(string email, string pass)
        {
            var result = await _userService.LoginAdmin(email, pass);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpGet("Login/Customer/{email}, {pass}")]
        public async Task<IActionResult> LoginCustomer(string email, string pass)
        {
            var result = await _userService.LoginCustomer(email, pass);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpGet("Login/Staff/{email}, {pass}")]
        public async Task<IActionResult> LoginStaff(string email, string pass)
        {
            var result = await _userService.LoginStaff(email, pass);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpPut("{id}")]
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
    }
}
