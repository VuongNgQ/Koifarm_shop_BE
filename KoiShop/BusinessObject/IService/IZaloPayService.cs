using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IZaloPayService
    {
        Task<Dictionary<string, string>> CreateZaloPayOrder(int orderId);
        Task<string> RefundOrder(string zpTransId, decimal amount, string description);
        //Task<Dictionary<string, object>> HandleCallbackAsync(dynamic cbdata);
        Task<Dictionary<string, object>> HandleCallbackAsync(ZaloPayCallbackRequestDTO cbdata);
    }
}
