using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.ReportModels
{
    public class ReportCreateModel
    {
        public string Description { get; set; }
        public string? FileUrl { get; set; } 
        public Guid ReporterId { get; set; } 
        public Guid ArtworkId { get; set; } 
    }
}
