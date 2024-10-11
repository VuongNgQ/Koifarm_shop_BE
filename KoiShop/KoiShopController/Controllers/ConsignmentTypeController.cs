using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsignmentTypeController : ControllerBase
    {
        private readonly IConsignmentTypeService _consignmentTypeService;
        public ConsignmentTypeController(IConsignmentTypeService service)
        {
            _consignmentTypeService = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetTypes(int page = 1, int pageSize = 10,
            string search = "", string sort = "")
        {
            var result = await _consignmentTypeService.GetTypes(page, pageSize, search, sort);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult>CreateType(CreateConsignmentTypeDTO typeDTO)
        {
            var result=await _consignmentTypeService.CreateType(typeDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            var result = await _consignmentTypeService.DeleteType(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
