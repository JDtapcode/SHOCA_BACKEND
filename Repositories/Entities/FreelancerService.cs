using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class FreelancerService:BaseEntity
    {
        public string Servicename { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int? DeliveryTime { get; set; }
        public int NumConcepts { get; set; }
        public int NumRevisions { get; set; }
        public Guid UserId { get; set; }

        public virtual Account? User { get; set; } 
        public virtual Category? Category { get; set; }
    }

}
