using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.RatingModels
{
    public class RatingCommentCreateModel
    {
        public Guid AccountId { get; set; }
        public Guid RatingId { get; set; }
        public string? CommentText { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
