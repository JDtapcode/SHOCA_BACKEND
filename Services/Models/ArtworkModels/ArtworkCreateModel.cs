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
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public Guid? CreatorId { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public List<Guid> CategoryIds { get; set; }
    }
}
