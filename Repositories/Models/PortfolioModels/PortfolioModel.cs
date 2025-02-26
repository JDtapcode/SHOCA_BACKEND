using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.PortfolioModels
{
    public class PortfolioModel:BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public Guid UserId { get; set; }
        public string? Skills { get; set; }
        public string? Experience { get; set; }
        public string? ContactUrl { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}
