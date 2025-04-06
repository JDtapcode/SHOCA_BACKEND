using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Report : BaseEntity
    {
        public string Description { get; set; } 
        public string? FileUrl { get; set; }
        public Guid? ReporterId { get; set; } 
        public Account? Reporter { get; set; }
        public Guid? ArtworkId { get; set; }
        public Artwork? Artwork { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Pending; 
    }
}
