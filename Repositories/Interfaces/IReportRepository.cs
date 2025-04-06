using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<(List<Report> Data, int TotalCount)> GetReportsByUserAsync(Guid userId, bool isDeleted = false);
        Task<(List<Report> Data, int TotalCount)> GetReportsByArtworkAsync(Guid artworkId, bool isDeleted = false);
        Task<(List<Report> Data, int TotalCount)> GetAllReportsAsync(bool includeDeleted = false);
    }
}
