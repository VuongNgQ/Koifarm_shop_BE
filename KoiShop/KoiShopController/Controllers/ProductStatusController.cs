using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/FishStatus")]
    public class ProductStatusController : Controller
    {
        private readonly IProductStatusService _service;
        public ProductStatusController(IProductStatusService service)
        {
            _service = service;   
        }
        [HttpPost]
        public async Task<IActionResult> CreateStatus(CreateFishStatusDTO statusDTO)
        {
            var result=await _service.CreateFishStatus(statusDTO);
            if(result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetFishStatus(int page = 1, int pageSize = 10,
            string search = "", string sort = "")
        {
            var result= await _service.GetFishStatuses(page, pageSize, search, sort);
            if(result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpGet("StatusID/{id}")]
        public async Task<IActionResult> GetStatusById(int id)
        {
            var result = await _service.GetStatusById(id);
            if(result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpGet("StatusName/{name}")]
        public  async Task<IActionResult> GetStatusByName(string name)
        {
            var result=await _service.GetStatusByName(name);
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
