using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.PayOSModels
{
    public class PaymentData
    {
        public string? CheckoutUrl { get; set; }
        public string? PaymentId { get; set; }
    }
}
