using BusinessObject.Configuration;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
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

        public ZaloPayService(IOptions<ZaloPayConfig> config, HttpClient httpClient)
        {
            _config = config.Value;
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
        public async Task<JObject> SendPostAsync(string url, Dictionary<string, string> formData)
        {
            // Tạo đối tượng FormUrlEncodedContent, nó sẽ tự động thêm Content-Type header
            var content = new FormUrlEncodedContent(formData);

            try
            {
                // Gửi yêu cầu POST
                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error: BAD_REQUEST " + await response.Content.ReadAsStringAsync());
                    return null;
                }

                // Đọc nội dung phản hồi và chuyển đổi sang JObject
                var responseString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseString);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Request error: " + e.Message);
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine("JSON error: " + e.Message);
                return null;
            }
        }

        static string app_id = "2553";
        static string key1 = "PcY4iZIKFCIdgZvA6ueMcMHHUbRLYjPL";
        static string create_order_url = "https://sb-openapi.zalopay.vn/v2/create";

        public async Task<Dictionary<string, string>> CreateZaloPayOrder(int orderId)
        {
            Random rnd = new Random();
            var embed_data = new { orderId = orderId };
            var items = new[] { new { item_name = "Sample Item", item_quantity = 1, item_price = 50000 } }; // Sample item

            var app_trans_id = rnd.Next(1000000); // Generate a random order's ID.
            var param = new Dictionary<string, string>
    {
        { "app_id", app_id },
        { "app_user", "user123" },
        { "app_time", Utils.Util.GetTimeStamp().ToString() },
        { "amount", "50000" },
        { "app_trans_id", DateTime.Now.ToString("yyMMdd") + "_" + app_trans_id },
        { "embed_data", JsonConvert.SerializeObject(embed_data) },
        { "item", JsonConvert.SerializeObject(items) },
        { "description", "Lazada - Thanh toán đơn hàng #" + app_trans_id },
        { "bank_code", "zalopayapp" }
    };

            var data = app_id + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                       + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));

            var result = await HttpHelper.PostFormAsync(create_order_url, param);
            var stringResult = result.ToDictionary(entry => entry.Key, entry => entry.Value?.ToString());

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
        public async Task<bool> HandleCallbackAsync(ZaloPayCallbackRequestDTO callbackRequest)
        {
            //var data = request.data;
            //var macLocal = GenerateSignature(data, _configuration["ZaloPaySettings:Key2"]);
            //return macLocal == request.mac;
            //var key2 = _configuration["ZaloPaySettings:Key2"];
            //var data = $"{callbackRequest.AppId}|{callbackRequest.AppTransId}|{callbackRequest.Amount}|{callbackRequest.AppTime}|{callbackRequest.ResultCode}";
            //var mac = GenerateSignature(data, key2);

            //if (mac != callbackRequest.Mac)
            //{
            //    return false;
            //}
            //if (callbackRequest.ResultCode == 1) // 1 = Thanh toán thành công
            //{
            //    // Cập nhật trạng thái đơn hàng trong hệ thống của bạn
            //    // Logic cập nhật đơn hàng như đổi trạng thái đơn hàng thành "Đã thanh toán"
            //    return true;
            //}

            return false;
        }
    }
}
