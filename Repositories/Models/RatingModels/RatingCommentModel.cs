using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.RatingModels
{
    public class RatingCommentModel : BaseEntity
    {
        public int RatingValue { get; set; }
        public string? CommentText { get; set; }
        public DateTime? CommentedOn { get; set; }
        public string? AccountName { get; set; }
        public Guid? ParentCommentId { get; set; }
        public List<RatingCommentModel> ChildComments { get; set; } = new List<RatingCommentModel>();
    }
}
