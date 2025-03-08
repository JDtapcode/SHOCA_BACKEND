using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.PortfolioModels
{
    public class PortfolioCreateModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public Guid UserId { get; set; }
        public string? Skills { get; set; }
        public string? Experience { get; set; }
        public string? ContactUrl { get; set; }
        //public List<PortfolioImageModel> Images { get; set; } = new();
        public List<Guid> ArtworkImageIds { get; set; }
    }
}
