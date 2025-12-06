using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using BarberShop.Helpers;

namespace BarberShop.Helpers
{
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>();
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.ContainsKey(key) ? _responseData[key] : string.Empty;
        }

        // ✅ Tạo URL thanh toán gửi đến VNPAY
        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (var kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(HttpUtility.UrlEncode(kv.Key) + "=" + HttpUtility.UrlEncode(kv.Value) + "&");
                }
            }

            string rawData = data.ToString().TrimEnd('&');
            string signData = HmacSHA512(vnp_HashSecret, rawData);
            string paymentUrl = $"{baseUrl}?{rawData}&vnp_SecureHash={signData}";
            return paymentUrl;
        }

        // ✅ Xác thực chữ ký phản hồi từ VNPAY
        public bool ValidateSignature(string inputHash, string secretKey)
        {
            string rawData = string.Join('&', _responseData
                .Where(kv => kv.Key.StartsWith("vnp_") && kv.Key != "vnp_SecureHash" && kv.Key != "vnp_SecureHashType")
                .Select(kv => $"{kv.Key}={kv.Value}"));

            string computedHash = HmacSHA512(secretKey, rawData);
            return string.Equals(computedHash, inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string HmacSHA512(string key, string inputData)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using var hmac = new HMACSHA512(keyBytes);
            byte[] hashBytes = hmac.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
