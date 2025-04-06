using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class RatingComment : BaseEntity
    {
        public string? CommentText { get; set; }
        public DateTime? CommentedOn { get; set; }

        public Guid RatingId { get; set; }
        public virtual Rating? Rating { get; set; }

        public Guid AccountId { get; set; }
        public virtual Account? Account { get; set; }

        public Guid? ParentCommentId { get; set; }
        public virtual RatingComment? ParentComment { get; set; }

        public virtual ICollection<RatingComment> ChildComments { get; set; } = new List<RatingComment>();
    }
}
