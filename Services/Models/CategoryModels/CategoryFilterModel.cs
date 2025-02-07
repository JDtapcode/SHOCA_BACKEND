using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.CategoryModels
{
    public class CategoryFilterModel : PaginationParameter
    {
        public bool isDelete { get; set; } = false;
        public string? Name { get; set; }

    }
}
