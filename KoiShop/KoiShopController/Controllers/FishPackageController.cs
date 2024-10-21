using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.RequestDTO.UpdateReq.Entity;
using BusinessObject.Model.ResponseDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FishPackageController : Controller
    {
        private readonly IFishPackageService _service;
        public FishPackageController(IFishPackageService service)
        {
            _service= service;
        }
        [HttpGet]
        public async Task<IActionResult> GetPackages(int page = 1, int pageSize = 10,
            string search = "", string sort = "")
        {
            var result = await _service.GetFishPackages(page, pageSize, search, sort);
            if(!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById([FromRoute] int id)
        {
            var result = await _service.GetFishPackage(id);
            if( result == null )
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromForm]CreateFishPackageDTO packageDTO)
        {
            var result=await _service.CreatePackage(packageDTO);
            if( !result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage([FromRoute]int id)
        {
            var result = await _service.DeletePackage(id);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage([FromRoute] int id, [FromForm] UpdatePackageDTO packageDTO)
        {
            var result = await _service.UpdatePackage(id, packageDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
    
}
