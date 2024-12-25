using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Artwork:BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string FileUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int CreatorId { get; set; }
        public int PortfolioId { get; set; }
    }
}
