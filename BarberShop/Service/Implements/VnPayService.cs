    using BarberShop.Configurations;
    using BarberShop.Services.Interfaces;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using BarberShop.Helpers;

    namespace BarberShop.Services.Implements
    {
        public class VnPayService : IVnPayService
        {
            private readonly VnPaySettings _settings;

            public VnPayService(IOptions<VnPaySettings> options)
            {
                _settings = options.Value;
            }

            public string CreatePaymentUrl(decimal amount, string orderInfo, string ipAddress)
            {
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _settings.TmnCode);

            // amount phải là số nguyên (long) nhân 100
            vnpay.AddRequestData("vnp_Amount", ((long)(amount * 100)).ToString());

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", ipAddress);
            vnpay.AddRequestData("vnp_Locale", "vn");

            // encode orderInfo
            vnpay.AddRequestData("vnp_OrderInfo", Uri.EscapeDataString(orderInfo));

            // URL trả về phải đúng
            vnpay.AddRequestData("vnp_ReturnUrl", _settings.ReturnUrl);

            // tạo mã tham chiếu
            vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

            // tạo URL thanh toán
            string paymentUrl = vnpay.CreateRequestUrl(_settings.BaseUrl, _settings.HashSecret);

            return paymentUrl;
        }

            public bool ValidateSignature(NameValueCollection queryParams)
            {
                var vnpay = new VnPayLibrary();
                foreach (string key in queryParams)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, queryParams[key]);
                    }
                }

                string inputHash = queryParams["vnp_SecureHash"];
                return vnpay.ValidateSignature(inputHash, _settings.HashSecret);
            }
        }
    }
