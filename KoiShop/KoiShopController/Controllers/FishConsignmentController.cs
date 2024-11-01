using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FishConsignmentController : ControllerBase
    {
        private readonly IFishConsignmentService _consignmentService;

        public FishConsignmentController(IFishConsignmentService consignmentService)
        {
            _consignmentService = consignmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateConsignment([FromBody] CreateConsignmentDTO consignmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _consignmentService.CreateConsignment(consignmentDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsignmentById(int id)
        {
            var result = await _consignmentService.GetConsignmentById(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConsignments()
        {
            var result = await _consignmentService.GetAllConsignments();
            return Ok(result.Data);
        }
    }

}
