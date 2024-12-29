using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class ProPackage:BaseEntity
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Feature { get; set; }
        public string? Duration { get; set; }

        public virtual ICollection<AccountProPackage> AccountProPackages { get; set; } = new List<AccountProPackage>();
    }

}
