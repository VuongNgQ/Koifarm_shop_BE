using BusinessObject.IService;
using DataAccess.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [Route("api/Search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IFishPackageService _packageService;
        public SearchController(IFishPackageService service)
        {
            _packageService = service;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <param name="sort">name, price, minsize, maxsize, fishinpackage</param>
        /// <param name="productStatus">AVAILABLE, UNAVAILABLE, SOLDOUT</param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        [HttpGet("SearchAll")]
        public async Task<IActionResult> GetAllOrder(int page = 1, int pageSize = 10, string? search = "", string sort = "",
            string? productStatus = null, decimal? minSize = null, decimal? maxSize = null,
    decimal? minPrice = null, decimal? maxPrice = null)
        {
            var result = await _packageService.SearchFishAndPackages(page, pageSize, search, sort, productStatus, minSize, maxSize, minPrice, maxPrice);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
    }
}
