using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IZaloPayService
    {
        Task<ZaloPayCreateOrderResponseDTO> CreateOrder(decimal amount, string orderId, string description);
        Task<string> RefundOrder(string zpTransId, decimal amount, string description);
        bool VerifyCallback(ZaloPayCallbackRequestDTO request);
    }
}
