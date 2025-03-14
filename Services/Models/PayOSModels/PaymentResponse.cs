using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.PayOSModels
{
    public class PaymentResponse
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public PaymentData Data { get; set; }
    }
}
