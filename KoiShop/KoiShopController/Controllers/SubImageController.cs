using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubImageController : ControllerBase
    {
        private readonly ISubImageService _service;
        public SubImageController(ISubImageService service)
        {
            _service = service;
        }
        [HttpGet("Fish/{id}")]
        public async Task<IActionResult> GetByFishId([FromRoute]int id)
        {
            var result=await _service.GetSubImageByFishId(id);
            if(result.Success)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }
        [HttpGet("Package/{id}")]
        public async Task<IActionResult> GetByPackageId([FromRoute] int id)
        {
            var result = await _service.GetSubImageByPackageId(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }
        [HttpPost("Fish")]
        public async Task<IActionResult> CreateForFish([FromForm] CreateFishSubImageDTO imageDTO)
        {
            var result = await _service.CreateFishSubImage(imageDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPost("Package")]
        public async Task<IActionResult> CreateForPackage([FromForm] CreatePackageSubImageDTO imageDTO)
        {
            var result = await _service.CreatePackageSubImage(imageDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPut("{imageId}")]
        public async Task<IActionResult> ChangeImage([FromRoute]int imageId, [FromForm]IFormFile image)
        {
            var result = await _service.ChangeSubImage(imageId, image);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage([FromRoute]int imageId)
        {
            var result=await _service.DeleteSubImage(imageId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
