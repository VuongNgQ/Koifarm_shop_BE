using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
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
        /// <summary>
        /// Ongoing
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(CreateUserDTO newUser)
        {
            var result = await _userService.CreateUser(newUser);
            return Ok(result);
        }
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            var result = await _userService.GetAllUser();
            return Ok(result);
        }
    }
}
