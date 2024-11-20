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
        /// <summary>
        /// Get consignment by consignmentId.
        /// </summary>
        /// <param name="id">Consignment Id</param>
        /// <returns>The consignment information.</returns>
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

        /// <summary>
        /// Get all consignment.
        /// </summary>
        /// <returns>The list of consignment.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllConsignments()
        {
            var result = await _consignmentService.GetAllConsignmentsAsync();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Get list of consignment by userId.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>The list of consignment information.</returns>
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
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Create care consignment request with boughten fish.
        /// </summary>
        /// <param name="consignmentDto">Details of Consignment</param>
        /// <returns>.</returns>
        [HttpPost("care")]
        public async Task<IActionResult> CreateCareConsignment([FromForm] CareConsignmentDTO consignmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _consignmentService.CreateCareConsignmentAsync(consignmentDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        /// <summary>
        /// Create sale consignment request.
        /// </summary>
        /// <param name="consignmentDto">Details of Consignment</param>
        /// <returns>.</returns>
        [HttpPost("sale")]
        public async Task<IActionResult> CreateSaleConsignment([FromForm] SaleConsignmentDTO consignmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _consignmentService.CreateSaleConsignmentAsync(consignmentDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
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

        /// <summary>
        /// Approve consignment request.
        /// </summary>
        /// /// <param name="consignmentId">Consignment Id</param>
        /// <param name="approveDTO">Price( for sale)</param>
        /// <returns>.</returns>
        [HttpPost("approve/{consignmentId}")]
        public async Task<IActionResult> ApproveConsignment(int consignmentId, [FromBody] ApproveConsignmentDTO approveDTO)
        {
            var response = await _consignmentService.ApproveConsignmentAsync(consignmentId, approveDTO);
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
