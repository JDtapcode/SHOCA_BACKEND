using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.PayOSModels
{
    public class PayOSPaymentResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string PaymentUrl { get; set; }
    }
}
