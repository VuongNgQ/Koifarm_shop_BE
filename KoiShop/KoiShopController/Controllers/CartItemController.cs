using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _service;
        public CartItemController(ICartItemService service)
        {
            _service = service;
        }
        /*[HttpGet]
        public async Task<IActionResult>GetAllItems(int page = 1, int pageSize = 10,
            string search = "", string sort = "")
        {
            var result=await _service.GetAllItem(page, pageSize, search, sort);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }*/
        /// <summary>
        /// Get Item by Cart ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A list of Item in the Cart</returns>
        [HttpGet("Cart/{id}")]
        public async Task<IActionResult>GetByCartId(int id)
        {
            var result=await _service.GetItemByCartId(id);
            if(result.Success)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }
        /// <summary>
        /// Create Fish Item 
        /// </summary>
        /// <param name="itemDTO"></param>
        /// <returns>Fish Item in Cart</returns>
        [HttpPost("FishSingle")]
        public async Task<IActionResult>CreateFishItem(CreateFishItemDTO itemDTO)
        {
            var result=await _service.CreateFishItem(itemDTO);  
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        /// <summary>
        /// Create Package Item
        /// </summary>
        /// <param name="itemDTO"></param>
        /// <returns>Package Item in Cart</returns>
        [HttpPost("FishPackage")]
        public async Task<IActionResult>CreatePackageItem(CreatePackageItemDTO itemDTO)
        {
            var result=await _service.CreatePackageItem(itemDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        /// <summary>
        /// Delete Item with Item ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result= await _service.DeleteCartItemById(id);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        /// <summary>
        /// Update Quantity for Package Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPut("PackageQuantity/{id}&&{quantity}")]
        public async Task<IActionResult> UpdateFishQuantity(int id, int quantity)
        {
            var result = await _service.UpdatePackageQuantity(id, quantity);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
