using BusinessObject.Configuration;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.IRepo;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly ZaloPayConfig _config;
        private readonly HttpClient _httpClient;
        private readonly IOrderService _orderService;
        private readonly IOrderRepo _orderRepo;
        private readonly IPaymentRepo _paymentRepo;

        public ZaloPayService(IOptions<ZaloPayConfig> config, IOrderService orderService, IOrderRepo orderRepo, IPaymentRepo paymentRepo, HttpClient httpClient)
        {
            _config = config.Value;
            _orderService = orderService;
            _orderRepo = orderRepo;
            _httpClient = httpClient;
            _paymentRepo = paymentRepo;
        }

        static string app_id = "2554";
        static string create_order_url = "https://sb-openapi.zalopay.vn/v2/create";

        public async Task<Dictionary<string, string>> CreateZaloPayOrder(int orderId)
        {

            var orderResponse = await _orderService.GetOrderById(orderId);
            if (orderResponse == null)
            {
                throw new ArgumentException("Order or Order items not found.");
            }
            var order = orderResponse.Data;
            var amount = (int)order.TotalPrice;
            var items = order.Items.Select(item => new
            {
                item_name = item.FishName ?? item.PackageName ?? "Unknown item",
                item_quantity = item.Quantity,
                item_price = (int)item.Price
            }).ToArray();
            Random rnd = new Random();
            var embed_data = new { orderId = orderId};
            var callbackUrl = "https://7b3b-118-69-182-144.ngrok-free.app/api/Payment/zalopay-callback";
            var app_trans_id = rnd.Next(1000000);

            var payment = new Payment
            {
                Amount = order.TotalPrice,
                OrderId = orderId,
                TransactionId = app_trans_id,
                Currency = "VND",
                TransactionType = TransactionType.BuyFish,
                PaymentStatus = PaymentStatus.Pending,
                Description = "KoiFarmShop - Thanh toán đơn hàng #" + app_trans_id
            };
            await _paymentRepo.AddPaymentAsync(payment);
            var param = new Dictionary<string, string>
            {
                { "app_id", _config.AppId },
                { "app_user", order.UserId.ToString() },
                { "app_time", Utils.Util.GetTimeStamp().ToString() },
                { "amount", amount.ToString() },
                { "app_trans_id", DateTime.Now.ToString("yyMMdd") + "_" + app_trans_id },
                { "embed_data", JsonConvert.SerializeObject(embed_data) },
                { "item", JsonConvert.SerializeObject(items) },
                {"callback_url", callbackUrl },
                { "description", payment.Description },
                { "bank_code", "" }
            };

            var data = _config.AppId + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                       + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _config.Key1, data));

            var result = await HttpHelper.PostFormAsync(create_order_url, param);
            var stringResult = result.ToDictionary(entry => entry.Key, entry => entry.Value?.ToString());
            if (stringResult.TryGetValue("order_url", out var orderUrl))
            {
                payment.PaymentUrl = orderUrl;
                await _paymentRepo.UpdatePaymentAsync(payment);
            }
            return stringResult;
        }

        public async Task<string> RefundOrder(string zpTransId, decimal amount, string description)
        {
        //    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        //    var data = $"{_configuration["ZaloPaySettings:AppId"]}|{zpTransId}|{amount}|{timestamp}";
        //    var mac = GenerateSignature(data, _configuration["ZaloPaySettings:Key1"]);

        //    var requestData = new Dictionary<string, object>
        //{
        //    { "appid", _configuration["ZaloPaySettings:AppId"] },
        //    { "zptransid", zpTransId },
        //    { "amount", amount },
        //    { "description", description },
        //    { "timestamp", timestamp },
        //    { "mac", mac }
        //};

        //    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync(_configuration["ZaloPaySettings:RefundEndpoint"], content);
        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"Error processing refund with ZaloPay: {responseContent}");
        //    }

            return null;
        }
        public async Task<Dictionary<string, object>> HandleCallbackAsync(ZaloPayCallbackRequestDTO cbdata)
        {
            var result = new Dictionary<string, object>();

            try
            {
                //var dataStr = Convert.ToString(cbdata["data"]);
                //var reqMac = Convert.ToString(cbdata["mac"]);
                var dataStr = cbdata.Data;
                var reqMac = cbdata.Mac;
                var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _config.Key2, dataStr);

                if (!reqMac.Equals(mac))
                {
                    result["return_code"] = -1;
                    result["return_message"] = "mac not equal";
                    return result;
                }
                var isSuccess = cbdata.Type == 1;
                var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
                var appTransId = Convert.ToString(dataJson["app_trans_id"]);
                //var actualReturnCode = dataJson.ContainsKey("return_code") ? Convert.ToInt32(dataJson["return_code"]) : -1;
                //var isSuccess = actualReturnCode == 1;

                var payment = await _paymentRepo.GetPaymentByTransactionIdAsync(int.Parse(appTransId.Split('_')[1]));
                if (payment == null)
                {
                    result["return_code"] = -1;
                    result["return_message"] = "Payment not found";
                    return result;
                }

                payment.PaymentStatus = isSuccess ? PaymentStatus.Completed : PaymentStatus.Failed;
                payment.PaymentDate = DateTime.Now;
                await _paymentRepo.UpdatePaymentAsync(payment);

                var order = await _orderRepo.GetByIdAsync(payment.OrderId.Value);
                if (order != null)
                {
                    order.Status = isSuccess ? OrderStatusEnum.COMPLETED : OrderStatusEnum.CANCELLED;
                    order.CompleteDate = DateTime.Now;
                    await _orderRepo.UpdateOrder(order);
                }

                result["return_code"] = 1;
                result["return_message"] = "success";
            }
            catch (Exception ex)
            {
                result["return_code"] = 0;
                result["return_message"] = ex.Message;
            }

            return result;
        }
    }
}
