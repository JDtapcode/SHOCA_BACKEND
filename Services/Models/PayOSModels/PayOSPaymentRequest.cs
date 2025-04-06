using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.PayOSModels
{
    public class PayOSPaymentRequest
    {
        public string OrderCode { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public string Signature { get; set; }
    }
}
