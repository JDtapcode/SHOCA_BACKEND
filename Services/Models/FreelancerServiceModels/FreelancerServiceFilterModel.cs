using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.FreelancerServiceModels
{
    public class FreelancerServiceFilterModel : PaginationParameter
    {
        public bool isDelete { get; set; } = false;
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int? MinDeliveryTime { get; set; }
        public int? MaxDeliveryTime { get; set; }
        public int? NumConcepts { get; set; }
        public int? NumRevisions { get; set; }
        public Guid UserId { get; set; }
    }
}
