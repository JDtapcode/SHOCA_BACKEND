using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Artwork:BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public int? LikeNumber { get; set; }
        public Guid? CreatorId { get; set; }
        public Account? Creator { get; set; }
        public ArtworkStatus? Status { get; set; } = ArtworkStatus.Pending;
        public virtual ICollection<ArtworkImage> Images { get; set; } = new List<ArtworkImage>();
        public virtual ICollection<ArtworkCategory> ArtworkCategories { get; set; } = new List<ArtworkCategory>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
