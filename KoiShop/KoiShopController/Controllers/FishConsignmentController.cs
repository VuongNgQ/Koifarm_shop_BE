using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
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

            var result = await _consignmentService.CreateConsignmentAsync(consignmentDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsignmentById(int id)
        {
            var result = await _consignmentService.GetConsignmentByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConsignments()
        {
            var result = await _consignmentService.GetAllConsignmentsAsync();
            return Ok(result.Data);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetConsignmentsByUserId(int userId)
        {
            var result = await _consignmentService.GetConsignmentsByUserIdAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateConsignment([FromBody] FishConsignmentDTO consignmentDto)
        {
            var response = await _consignmentService.UpdateConsignmentAsync(consignmentDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveConsignment(int id, [FromBody] ApproveConsignmentDTO approveDTO)
        {
            var response = await _consignmentService.ApproveConsignmentAsync(id, approveDTO);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("{id}/complete-sale")]
        public async Task<IActionResult> CompleteSaleConsignment(int id)
        {
            var response = await _consignmentService.CompleteSaleConsignmentAsync(id);
            if (response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }

}
