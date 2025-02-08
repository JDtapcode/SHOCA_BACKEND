using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.ArtworkModels
{
    public class ArtworkCreateModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string FileUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public Guid CreatorId { get; set; }
        public Guid? PortfolioId { get; set; }
        public List<Guid> CategoryIds { get; set; }
    }
}
