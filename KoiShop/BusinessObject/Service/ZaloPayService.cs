using BusinessObject.Configuration;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
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


        public async Task<ZaloPayCreateOrderResponseDTO> CreateOrder(ZaloPayRequestDTO request)
        {
            var appId = _configuration["ZaloPaySettings:AppId"];
            var key1 = _configuration["ZaloPaySettings:Key1"];

            // Lấy thời gian và các thông tin cần thiết
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var vietnamTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, vietnamTimeZone);
            var transId = $"{vietnamTime:yyMMdd}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

            var appTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var embedData = "";
            var items = "[{\"itemid\":\"knb\",\"itemname\":\"kim nguyen bao\",\"itemquantity\":10,\"itemprice\":50000}]";

            // Tạo chữ ký HMAC
            var data = $"{appId}|{transId}|{_configuration["ZaloPaySettings:AppUser"]}|{request.Amount}|{appTime}|{embedData}|{items}";
            var hmac = GenerateSignature(data, key1);

            // Chuẩn bị dữ liệu dưới dạng Dictionary cho form-urlencoded
            var formData = new Dictionary<string, string>
    {
        { "app_id", appId },
        { "app_user", _configuration["ZaloPaySettings:AppUser"] },
        { "app_time", appTime },
        { "amount", request.Amount.ToString() },
        { "bank_code", "" },
        { "app_trans_id", transId },
        { "embed_data", embedData },
        { "item", items },
        { "callback_url", "" },
        { "description", request.Description },
        { "mac", hmac }
    };
            Console.WriteLine(JsonConvert.SerializeObject(formData));

            // Gọi phương thức SendPostAsync để gửi yêu cầu
            var responseJson = await SendPostAsync(_configuration["ZaloPaySettings:Endpoint"], formData);

            if (responseJson == null)
            {
                throw new Exception("Error creating ZaloPay order.");
            }

            // Deserialize response JSON thành đối tượng ZaloPayCreateOrderResponseDTO
            var zaloResponse = responseJson.ToObject<ZaloPayCreateOrderResponseDTO>();
            return zaloResponse;
        }

        //public async Task<ZaloPayCreateOrderResponseDTO> CreateOrder(ZaloPayRequestDTO request)
        //{
        //    var appId = _configuration["ZaloPaySettings:AppId"];
        //    var key1 = _configuration["ZaloPaySettings:Key1"];
        //    // Lấy múi giờ Việt Nam
        //    var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); var vietnamTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, vietnamTimeZone);
        //    //var transId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        //    var transId = $"{vietnamTime:yyMMdd}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        //    Random rnd = new Random();
        //    var app_trans_id = rnd.Next(1000000);
        //    var user_id = _configuration["ZaloPaySettings:AppUser"];
        //    //var transId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString("yyMMdd") + "_" + app_trans_id);
        //    var appTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        //    var embedData = "";
        //    var items = "[{\"itemid\":\"knb\",\"itemname\":\"kim nguyen bao\",\"itemquantity\":10,\"itemprice\":50000}]";
        //    var data = $"{appId}|{transId}|{user_id}|{request.Amount}|{appTime}|{embedData}|{items}";
        //    var hmac = GenerateSignature(data, _configuration["ZaloPaySettings:Key1"]);
        //    var paymentData = new
        //    {
        //        app_id = appId,
        //        app_user = _configuration["ZaloPaySettings:AppUser"],
        //        app_time = appTime,
        //        amount = request.Amount,
        //        bank_code = "",
        //        app_trans_id = transId,
        //        embed_data = embedData,
        //        item = items,
        //        callback_url = "",
        //        description = request.Description,
        //        mac = hmac
        //    };

        //    var content = new StringContent(JsonConvert.SerializeObject(paymentData), Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync(_configuration["ZaloPaySettings:Endpoint"], content);
        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"Error creating ZaloPay order: {responseContent}");
        //    }

        //    // Deserialize JSON response
        //    var zaloResponse = JsonConvert.DeserializeObject<ZaloPayCreateOrderResponseDTO>(responseContent);
        //    return zaloResponse;
        //}

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
