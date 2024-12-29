using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class ArtworkCategory : BaseEntity
    {
        public Guid ArtworkId { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Artwork? Artwork { get; set; }
        public virtual Category? Category { get; set; }
    }


}
