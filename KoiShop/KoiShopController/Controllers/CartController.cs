using BusinessObject.IService;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service)
        {
            _service = service;
        }
    }
}
