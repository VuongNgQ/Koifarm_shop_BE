using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
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
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            var result = await _service.GetFishPackage(id);
            if( result == null )
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePackage(CreateFishPackageDTO packageDTO)
        {
            var result=await _service.CreatePackage(packageDTO);
            if( result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var result = await _service.DeletePackage(id);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, ResponseFishPackageDTO packageDTO)
        {
            var result = await _service.UpdatePackage(id, packageDTO);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
    
}
