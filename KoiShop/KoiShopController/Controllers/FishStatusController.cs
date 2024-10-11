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
        public async Task<IActionResult> CreateFishStatus(CreateFishStatusDTO statusDTO)
        {
            var result = await _fishStatusService.CreateFishStatus(statusDTO);
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
        public async Task<IActionResult> GetFishStatuses(int page, int pageSize,
            string? search, string sort)
        {
            var result = await _fishStatusService.GetAllFishStatus(page, pageSize, search, sort);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFishStatusById(int id)
        {
            var result = await _fishStatusService.GetFishStatusById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFishStatusById(int id)
        {
            var result = await _fishStatusService.DeleteFishStatus(id);
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
