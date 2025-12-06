using System.Collections.Specialized;

namespace BarberShop.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(decimal amount, string orderInfo, string ipAddress);
        bool ValidateSignature(NameValueCollection queryParams);
    }
}
