using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductStatusController : ControllerBase
    {
        private readonly IProductStatusService _service;
        public ProductStatusController(IProductStatusService service)
        {
            _service = service;   
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductStatus(CreateProductStatusDTO statusDTO)
        {
            var result = await _service.CreateProductStatus(statusDTO);
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
        public async Task<IActionResult> GetProductStatuses(int page = 1, int pageSize = 10,
            string search = "", string sort = "")
        {
            var result = await _service.GetProductStatuses(page, pageSize, search, sort);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpGet("StatusID/{id}")]
        public async Task<IActionResult> GetProductStatusById(int id)
        {
            var result = await _service.GetStatusById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpGet("StatusName/{name}")]
        public async Task<IActionResult> GetStatusByName(string name)
        {
            var result = await _service.GetStatusByName(name);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpDelete("DeleteStatusWithId/{id}")]
        public async Task<IActionResult> DeleteStatusById(int id)
        {
            var result = await _service.DeleteProductStatus(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpDelete("DeleteStatusWithName/{name}")]
        public async Task<IActionResult> DeleteStatusByName(string name)
        {
            var result = await _service.DeleteStatusByName(name);
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
