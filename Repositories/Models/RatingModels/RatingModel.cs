using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.RatingModels
{
    public class RatingModel:BaseEntity
    {
        public string? Comments { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public Guid ArtworkId { get; set; }
        public string? ArtworkTitle { get; set; }
        public List<RatingCommentModel> CommentsList { get; set; } = new List<RatingCommentModel>();

    }
}
