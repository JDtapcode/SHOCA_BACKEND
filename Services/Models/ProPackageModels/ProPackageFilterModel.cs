using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.ProPackageModels
{
    public class ProPackageFilterModel:PaginationParameter
    {
        public bool isDeleted { get; set; } = false;
    }
}
