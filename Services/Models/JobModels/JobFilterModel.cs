using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.JobModels
{
    public class JobFilterModel : PaginationParameter
    {
        public bool isDelete { get; set; } = false;
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public string? Location {  get; set; }
        public string? Category { get; set; }
        public Guid UserId { get; set; }
    }
}
