using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.PayOSModels
{
    public class PaymentRequestModel
    {
        public Guid PackageId { get; set; }
        public Guid AccountId { get; set; }
    }
}
