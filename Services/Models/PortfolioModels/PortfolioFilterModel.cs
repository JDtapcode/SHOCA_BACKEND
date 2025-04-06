using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.PortfolioModels
{
    public class PortfolioFilterModel:PaginationParameter
    {
        public bool isDelete { get; set; } = false;
        public string? Skills { get; set; }
        public string? Experience { get; set; }
    }
}
