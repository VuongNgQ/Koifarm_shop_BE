using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusService _service;
        public OrderStatusController(IOrderStatusService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStatus(CreateOrderStatusDTO statusDTO)
        {
            var result = await _service.CreateOrderStatus(statusDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStatuses(int page = 1, int pageSize = 10,
            string search = "", string sort = "")
        {
            var result = await _service.GetAllOrdersStatus(page, pageSize, search, sort);
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
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var result = await _service.DeleteOrderStatusById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
