using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _service;
        public PaymentMethodController(IPaymentMethodService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePayment(CreatePaymentMethodDTO paymentDTO)
        {
            var result = await _service.CreatePayment(paymentDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPayments(int page = 1, int pageSize = 10,
            string search = "", string sort = "")
        {
            var result = await _service.GetPayments(page, pageSize, search, sort);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentById(int id)
        {
            var result = await _service.DeletePaymentById(id);
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
