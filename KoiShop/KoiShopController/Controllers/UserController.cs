using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class UserController:ControllerBase
    {
        public async Task<IActionResult> CreateUser(CreateUserDTO newUser)
        {

            return Ok(newUser);
        }
    }
}
