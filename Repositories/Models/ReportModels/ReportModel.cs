using Repositories.Enums;
using Repositories.Models.AccountModels;
using Repositories.Models.ArtworkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.ReportModels
{
    public class ReportModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string? FileUrl { get; set; }
        public Guid ReporterId { get; set; }
        public Guid ArtworkId { get; set; }
        public ReportStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public ArtworkModel Artwork { get; set; }     // <-- Thêm dòng này
        public AccountModel Reporter { get; set; }

    }
}
