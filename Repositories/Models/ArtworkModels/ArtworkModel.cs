using Repositories.Entities;
using Repositories.Models.CategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.ArtworkModels
{
    public class ArtworkModel: BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public int? LikeNumber { get; set; }
        public Guid CreatorId { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        //public List<CategoryModel> Categories { get; set; } = new();
        public List<string> Images { get; set; } = new List<string>();
    }
}
