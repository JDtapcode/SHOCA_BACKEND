using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Portfolio : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public Guid UserId { get; set; }
        public Account? User { get; set; }
        public string? Skills { get; set; }
        public string? Experience { get; set; }
        public string? ContactUrl { get; set; }
        public virtual ICollection<PortfolioImage> PortfolioImages { get; set; } = new List<PortfolioImage>();
    }

}
