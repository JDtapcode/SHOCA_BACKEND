using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.ArtworkModels
{
    public class ArtworkFilterModel:PaginationParameter
    {
        public string? Title { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? PortfolioId { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
