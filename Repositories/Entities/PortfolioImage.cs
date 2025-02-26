using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class PortfolioImage
    {
        public Guid PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        public Guid ArtworkImageId { get; set; }
        public ArtworkImage ArtworkImage { get; set; }
        public string ImageUrl { get; set; }
    }
}
