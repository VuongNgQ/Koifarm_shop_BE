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

        public async Task<ZaloPayCreateOrderResponseDTO> CreateOrder(decimal amount, string orderId, string description)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentException("OrderId cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be null or empty.");
            }
            var appTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var embedData = "{}";
            //var embedData = JsonConvert.SerializeObject(new { redirecturl = "https://yourwebsite.com/return" });
            var amountInVND = (int)(amount * 1000);
            var items = "[]";
            var data = $"{_configuration["ZaloPaySettings:AppId"]}|{orderId}|{amount}|{appTime}|{embedData}|{items}";
            var mac = GenerateSignature(data, _configuration["ZaloPaySettings:Key1"]);

            var requestData = new Dictionary<string, object>
        {
            { "appid", _configuration["ZaloPaySettings:AppId"] },
            { "apptransid", orderId },
            { "apptime", appTime },
            { "amount", amountInVND },
            { "appuser", "user123" },
            { "description", description },
            { "embeddata", embedData },
            { "item", items },
            { "mac", mac }
        };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_configuration["ZaloPaySettings:Endpoint"], content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error creating ZaloPay order: {responseContent}");
            }

            // Deserialize JSON response
            var zaloResponse = JsonConvert.DeserializeObject<ZaloPayCreateOrderResponseDTO>(responseContent);

            // Check return code from ZaloPay response
            if (zaloResponse.ReturnCode != 1)
            {
                throw new Exception($"ZaloPay returned error: {zaloResponse.ReturnMessage}");
            }

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
        public bool VerifyCallback(ZaloPayCallbackRequestDTO request)
        {
            var data = request.data;
            var macLocal = GenerateSignature(data, _configuration["ZaloPaySettings:Key2"]);
            return macLocal == request.mac;
        }
    }
}
