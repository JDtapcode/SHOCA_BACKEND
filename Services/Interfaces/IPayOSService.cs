using Services.Models.PayOSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPayOSService
    {
        Task<string> CreatePaymentUrlAsync(Guid packageId, Guid accountId);
        Task<PaymentResponseModel> HandlePaymentReturnAsync(string transactionId);
    }
}
