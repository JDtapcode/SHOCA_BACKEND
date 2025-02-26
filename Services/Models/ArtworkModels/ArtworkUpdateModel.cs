using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.ArtworkModels
{
    public class ArtworkUpdateModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string FileUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public List<Guid> CategoryIds { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();

    }
}
