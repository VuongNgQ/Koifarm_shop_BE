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
        /*[HttpPost]s
        public async Task<IActionResult>CreateStatus(CreateFishStatusDTO statusDTO)
        {
            var result=await _fishStatusService.CreateStatus(statusDTO);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }*/
    }
}
