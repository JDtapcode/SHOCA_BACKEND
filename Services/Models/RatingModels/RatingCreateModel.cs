using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.RatingModels
{
    public class RatingCreateModel
    {
        public Guid ArtworkId { get; set; }
        public int RatingValue { get; set; }
        public string? Comments { get; set; }
        public Guid CustomerId { get; set; }
    }
}
