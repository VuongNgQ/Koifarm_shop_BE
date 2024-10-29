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

        [HttpPost]
        public async Task<IActionResult> CreateFish([FromForm] CreateFishDTO fish)
        {
            var result = await _fishService.CreateFish(fish);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFish(int fishId,[FromForm] UpdateFishDTO updateFishDto)
        {
            var result = await _fishService.UpdateFish(fishId, updateFishDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

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
