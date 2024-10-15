using DataAccess.Entity;
using DataAccess.IRepo;
using DataAccess.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FishController : ControllerBase
    {
        private readonly IFishRepo _fishRepo;

        public FishController(IFishRepo fishRepository)
        {
            _fishRepo = fishRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFishes()
        {
            var fishes = await _fishRepo.GetAllFishesAsync();
            return Ok(fishes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFishById(int id)
        {
            var fish = await _fishRepo.GetFishByIdAsync(id);
            if (fish == null)
            {
                return NotFound();
            }
            return Ok(fish);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFish([FromBody] Fish fish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _fishRepo.AddFishAsync(fish);
            return CreatedAtAction(nameof(GetFishById), new { id = fish.FishId }, fish);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFish(int id, [FromBody] Fish fish)
        {
            if (id != fish.FishId || !ModelState.IsValid)
            {
                return BadRequest();
            }

            await _fishRepo.UpdateFishAsync(fish);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFish(int id)
        {
            await _fishRepo.DeleteFishAsync(id);
            return NoContent();
        }
    }

}
