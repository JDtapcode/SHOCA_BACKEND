using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Rating:BaseEntity
    {
        public string? Comments { get; set; }
        public int RatingValue { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ArtworkId { get; set; }
        public ICollection<RatingComment> CommentsList { get; set; } = new List<RatingComment>();
        public virtual Account? Customer { get; set; }
        public virtual Artwork? Artwork { get; set; }
    }

}
