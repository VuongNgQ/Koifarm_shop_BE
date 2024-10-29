using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using DataAccess.Entity;
using DataAccess.IRepo;
using DataAccess.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FishController : ControllerBase
    {
        private readonly IFishService _fishService;

        public FishController(IFishService fishService)
        {
            _fishService = fishService;
        }

        /// <summary>
        /// Get a list of all fishes.
        /// </summary>
        /// <returns>List of Fish objects.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllFishes()
        {
            var result = await _fishService.GetAllFishes();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }
        /// <summary>
        /// Get details of a specific fish by ID.
        /// </summary>
        /// <param name="id">ID of the fish to retrieve.</param>
        /// <returns>Details of the specified fish.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFishById(int fishId)
        {
            var result = await _fishService.GetFishById(fishId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }

        /// <summary>
        /// Create a new fish entry.
        /// </summary>
        /// <param name="fish">Fish object containing fish details.</param>
        /// <returns>The created fish object.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateFish([FromBody] CreateFishDTO fish)
        {
            var result = await _fishService.CreateFish(fish);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update an existing fish by ID.
        /// </summary>
        /// <param name="id">ID of the fish to update.</param>
        /// <param name="fish">Updated fish object.</param>
        /// <returns>The updated fish object.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFish(int fishId, [FromBody] UpdateFishDTO updateFishDto)
        {
            var result = await _fishService.UpdateFish(fishId, updateFishDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete a fish by ID.
        /// </summary>
        /// <param name="fishId">ID of the fish to delete.</param>
        /// <returns>Status of deletion.</returns>
        [HttpDelete("{fishId}")]
        public async Task<IActionResult> DeleteFish(int fishId)
        {
            var result = await _fishService.DeleteFish(fishId);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return NotFound(result.Message);
        }
    }

}
