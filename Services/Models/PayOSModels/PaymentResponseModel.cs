using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.PayOSModels
{
    public class PaymentResponseModel
    {
        public string OrderId { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Amount { get; set; }
    }

}
