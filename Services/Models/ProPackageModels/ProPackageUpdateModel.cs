using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.ProPackageModels
{
    public class ProPackageUpdateModel
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public List<string>? Features { get; set; }
        public string? Duration { get; set; }
    }
}
