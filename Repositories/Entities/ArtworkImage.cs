using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class ArtworkImage : BaseEntity
    {
        public string FileUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public Guid ArtworkId { get; set; }
        public Artwork? Artwork { get; set; }

        public virtual ICollection<PortfolioImage> PortfolioImages { get; set; } = new List<PortfolioImage>();
    }
}
