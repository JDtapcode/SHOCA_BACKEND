using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.ProPackages
{
    public class ProPackageModel : BaseEntity
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public List<string> Features { get; set; } 
        public string? Duration { get; set; }
    }
}
