using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FishStatusController : ControllerBase
    {

        private readonly IFishStatusService _fishStatusService;
        public FishStatusController(IFishStatusService service)
        {
            _fishStatusService = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateStatus(CreateFishStatusDTO statusDTO)
        {
            var result = await _fishStatusService.CreateStatus(statusDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpGet]
        public async Task<IActionResult> GetStatuses(int page, int pageSize,
            string? search, string sort)
        {
            var result = await _fishStatusService.GetAllStatus(page, pageSize, search, sort);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _fishStatusService.GetStatusById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteById(int id)
        {
            var result= await _fishStatusService.DeleteStatus(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
