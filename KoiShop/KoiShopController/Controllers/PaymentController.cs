using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Service;
using DataAccess.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace KoiShopController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IZaloPayService _zaloPayService;

        public PaymentController(IZaloPayService zaloPayService, IPaymentService paymentService)
        {
            _zaloPayService = zaloPayService;
            _paymentService = paymentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment != null)
                return Ok(payment);
            return NotFound("Payment not found.");
        }

        [HttpGet("getPayment/{id}")]
        public async Task<IActionResult> GetPaymentByUserId(int id)
        {
            var payments = await _paymentService.GetPaymentByUserIdAsync(id);
            if (payments != null)
                return Ok(payments);
            return NotFound("Payments not found.");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpPut("{id}/update-status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromQuery] PaymentStatus status)
        {
            try
            {
                await _paymentService.UpdatePaymentStatusAsync(id, status);
                return Ok("Payment status updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Tạo yêu cầu thanh toán qua ZaloPay cho đơn hàng hiện có.
        /// </summary>
        /// <returns>URL thanh toán của ZaloPay.</returns>
        [HttpPost("create-payment/{orderId}")]
        public async Task<IActionResult> CreatePayment(int orderId)
        {
            try
            {
                var result = await _zaloPayService.CreateZaloPayOrder(orderId);

                if (result == null || result.Count == 0)
                {
                    return BadRequest("Please try again.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Hoàn tiền giao dịch qua ZaloPay.
        /// </summary>
        /// <param name="zpTransId">Mã giao dịch ZaloPay cần hoàn tiền.</param>
        /// <param name="amount">Số tiền hoàn lại.</param>
        /// <param name="description">Mô tả lý do hoàn tiền.</param>
        /// <returns>Xác nhận hoàn tiền.</returns>
        [HttpPost("refundOrder")]
        public async Task<IActionResult> RefundOrder(string zpTransId, decimal amount, string description)
        {
            try
            {
                var result = await _zaloPayService.RefundOrder(zpTransId, amount, description);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("zalopay-callback")]
        public async Task<IActionResult> ZaloPayCallback([FromBody] ZaloPayCallbackRequestDTO cbdata)
        {
            try
            {
                var result = await _zaloPayService.HandleCallbackAsync(cbdata);
                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest(new { message = "Xác thực hoặc cập nhật không thành công" });
            }
        }
    }
}
