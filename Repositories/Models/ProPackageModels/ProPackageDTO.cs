using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.ProPackageModels
{
    public class ProPackageDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Duration { get; set; }
        public List<string> FeatureNames { get; set; } = new List<string>();
    }

}
