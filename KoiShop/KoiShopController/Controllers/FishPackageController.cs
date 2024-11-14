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
        /// <summary>
        /// Get All with search, sort and paging
        /// </summary>
        /// <param name="page">Number of pages</param>
        /// <param name="pageSize">Number of records in a page</param>
        /// <param name="search">Search with name</param>
        /// <param name="sort">By name, number of fishes in package, age, price</param>
        /// <returns>A list</returns>
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
        /// <summary>
        /// Get with Package ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A single package with the ID</returns>
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
        /// <summary>
        /// Create a package
        /// </summary>
        /// <param name="packageDTO"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Create a package
        /// </summary>
        /// <param name="packageDTO"></param>
        /// <returns></returns>
        [HttpPost("AddFish")]
        public async Task<IActionResult> AddToPackage([FromForm] CreateCategoryPackageDTO packageDTO)
        {
            var result = await _service.AddFishToPackage(packageDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        /// <summary>
        /// Delete a package with ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Update a package
        /// </summary>
        /// <param name="id"></param>
        /// <param name="packageDTO"></param>
        /// <returns></returns>
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
