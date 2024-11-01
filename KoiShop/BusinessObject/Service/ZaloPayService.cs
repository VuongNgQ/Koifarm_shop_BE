using BusinessObject.Configuration;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ZaloPayService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        private string GenerateSignature(string data, string key)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("Data cannot be null or empty", nameof(data));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            }
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public async Task<ZaloPayCreateOrderResponseDTO> CreateOrder(ZaloPayRequestDTO request)
        {
            var appId = _configuration["ZaloPaySettings:AppId"];
            var key1 = _configuration["ZaloPaySettings:Key1"];
            // Lấy múi giờ Việt Nam
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); var vietnamTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, vietnamTimeZone);
            //var transId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var transId = $"{vietnamTime:yyMMdd}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            var appTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var embedData = JsonConvert.SerializeObject(new { redirecturl = "your_redirect_url" });
            var items = "[{\"itemid\":\"knb\",\"itemname\":\"kim nguyen bao\",\"itemquantity\":10,\"itemprice\":50000}]";
            var data = $"{appId}|{transId}|user_id|{request.Amount}|{appTime}|{embedData}|{items}";
            var hmac = GenerateSignature(data, _configuration["ZaloPaySettings:Key1"]);
            //=====================
            //var appTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            //var embedData = "{}";
            ////var embedData = JsonConvert.SerializeObject(new { redirecturl = "https://yourwebsite.com/return" });
            //var amountInVND = (int)(amount * 1000);
            //var items = "[]";
            //var data = $"{_configuration["ZaloPaySettings:AppId"]}|{orderId}|{amount}|{appTime}|{embedData}|{items}";
            //var mac = GenerateSignature(data, _configuration["ZaloPaySettings:Key1"]);
            //var requestData = new Dictionary<string, object>
            //{
            //{ "appid", _configuration["ZaloPaySettings:AppId"] },
            //{ "apptransid", orderId },
            //{ "apptime", appTime },
            //{ "amount", amountInVND },
            //{ "appuser", "user123" },
            //{ "description", description },
            //{ "embeddata", embedData },
            //{ "item", items },
            //{ "mac", mac }
            //};
            var paymentData = new
            {
                app_id = appId,
                app_trans_id = transId,
                app_user = _configuration["ZaloPaySettings:AppUser"],
                amount = request.Amount,
                app_time = appTime,
                embed_data = embedData,
                item = items,
                description = request.Description,
                mac = hmac
            };

            var content = new StringContent(JsonConvert.SerializeObject(paymentData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_configuration["ZaloPaySettings:Endpoint"], content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error creating ZaloPay order: {responseContent}");
            }

            // Deserialize JSON response
            var zaloResponse = JsonConvert.DeserializeObject<ZaloPayCreateOrderResponseDTO>(responseContent);
            //if (zaloResponse.ReturnCode != 1)
            //{
            //    throw new Exception($"ZaloPay returned error: {zaloResponse.ReturnMessage}");
            //}

            return zaloResponse;
        }

        public async Task<string> RefundOrder(string zpTransId, decimal amount, string description)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var data = $"{_configuration["ZaloPaySettings:AppId"]}|{zpTransId}|{amount}|{timestamp}";
            var mac = GenerateSignature(data, _configuration["ZaloPaySettings:Key1"]);

            var requestData = new Dictionary<string, object>
        {
            { "appid", _configuration["ZaloPaySettings:AppId"] },
            { "zptransid", zpTransId },
            { "amount", amount },
            { "description", description },
            { "timestamp", timestamp },
            { "mac", mac }
        };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_configuration["ZaloPaySettings:RefundEndpoint"], content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error processing refund with ZaloPay: {responseContent}");
            }

            return responseContent;
        }
        public async Task<bool> HandleCallbackAsync(ZaloPayCallbackRequestDTO callbackRequest)
        {
            //var data = request.data;
            //var macLocal = GenerateSignature(data, _configuration["ZaloPaySettings:Key2"]);
            //return macLocal == request.mac;
            var key2 = _configuration["ZaloPaySettings:Key2"];
            var data = $"{callbackRequest.AppId}|{callbackRequest.AppTransId}|{callbackRequest.Amount}|{callbackRequest.AppTime}|{callbackRequest.ResultCode}";
            var mac = GenerateSignature(data, key2);

            if (mac != callbackRequest.Mac)
            {
                return false;
            }
            if (callbackRequest.ResultCode == 1) // 1 = Thanh toán thành công
            {
                // Cập nhật trạng thái đơn hàng trong hệ thống của bạn
                // Logic cập nhật đơn hàng như đổi trạng thái đơn hàng thành "Đã thanh toán"
                return true;
            }

            return false;
        }
    }
}
