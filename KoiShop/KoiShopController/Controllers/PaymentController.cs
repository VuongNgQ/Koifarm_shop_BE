using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiShopController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IZaloPayService _zaloPayService;
        private readonly IOrderService _orderService;

        public PaymentController(IZaloPayService zaloPayService, IOrderService orderService)
        {
            _zaloPayService = zaloPayService;
            _orderService = orderService;
        }

        /// <summary>
        /// Tạo yêu cầu thanh toán qua ZaloPay cho đơn hàng hiện có.
        /// </summary>
        /// <param name="orderId">ID của đơn hàng cần thanh toán.</param>
        /// <returns>URL thanh toán của ZaloPay.</returns>
        [HttpPost("createPayment/{orderId}")]
        //public async Task<IActionResult> CreatePayment(int orderId)
        //{
        //    var result = await _orderService.CreateZaloPayOrder(orderId);

        //    if (!result.Success)
        //    {
        //        return BadRequest(result.Message);
        //    }

        //    return Ok(new { PaymentUrl = result.Data });
        //}
        public async Task<IActionResult> CreatePayment([FromBody] ZaloPayRequestDTO request)
        {
            try
            {
                var result = await _zaloPayService.CreateOrder(request);
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
        public async Task<IActionResult> ZaloPayCallback([FromBody] ZaloPayCallbackRequestDTO request)
        {
            var result = await _zaloPayService.HandleCallbackAsync(request);
            if (result)
            {
                return Ok(new { message = "Cập nhật trạng thái đơn hàng thành công" });
            }
            return BadRequest(new { message = "Xác thực hoặc cập nhật không thành công" });
        }
    }
}
