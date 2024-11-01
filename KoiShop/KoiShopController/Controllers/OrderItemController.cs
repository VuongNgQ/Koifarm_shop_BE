using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _service;
        public OrderItemController(IOrderItemService service)
        {
            _service = service;
        }
        /// <summary>
        /// Get All Items from All Orders
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllItems(int page = 1, int pageSize = 10, string sort = "")
        {
            var result = await _service.GetAllItem(page, pageSize, sort);
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
        /// Get Items in a Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Order/{id}")]
        public async Task<IActionResult> GetByOrderId(int id)
        {
            var result = await _service.GetItemByOrderId(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }
        /// <summary>
        /// Create Fish items by bringing all Fish items in Cart to Order
        /// </summary>
        /// <param name="itemDTO"></param>
        /// <returns></returns>
        [HttpPost("FishSingle")]
        public async Task<IActionResult> CreateFishItem(CreateOrderItemDTO itemDTO)
        {
            var result = await _service.CreateFishItem(itemDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        /// <summary>
        /// Create Package items by bringing all Package items in Cart to Order
        /// </summary>
        /// <param name="itemDTO"></param>
        /// <returns></returns>
        [HttpPost("FishPackage")]
        public async Task<IActionResult> CreatePackageItem(CreateOrderItemDTO itemDTO)
        {
            var result = await _service.CreatePackageItem(itemDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteOrderItemById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }*/
        /*[HttpPut("FishSingle/{id}&&{quantity}")]
        public async Task<IActionResult> UpdateFishQuantity(int id, int quantity)
        {
            var result = await _service.UpdateFishQuantity(id, quantity);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }*/
    }
}
