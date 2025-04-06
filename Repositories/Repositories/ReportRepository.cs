using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        private readonly IClaimsService _claimsService;

        public ReportRepository(AppDbContext context, IClaimsService claimsService)
            : base(context, claimsService)
        {
            _claimsService = claimsService;
        }

        public async Task<(List<Report> Data, int TotalCount)> GetReportsByUserAsync(Guid userId, bool isDeleted = false)
        {
            Expression<Func<Report, bool>> filter = r => r.ReporterId == userId && r.IsDeleted == isDeleted;

            return await GetAllAsync(
                filter: filter,
                pageIndex: 1, 
                pageSize: int.MaxValue, 
                includes: new Expression<Func<Report, object>>[]
                {
                    r => r.Reporter,
                    r => r.Artwork
                });
        }

        public async Task<(List<Report> Data, int TotalCount)> GetReportsByArtworkAsync(Guid artworkId, bool isDeleted = false)
        {
            Expression<Func<Report, bool>> filter = r => r.Id == artworkId && r.IsDeleted == isDeleted;

            return await GetAllAsync(
                filter: filter,
                pageIndex: 1, 
                pageSize: int.MaxValue, 
                includes: new Expression<Func<Report, object>>[]
                {
                    r => r.Reporter,
                    r => r.Artwork
                });
        }
        public async Task<(List<Report> Data, int TotalCount)> GetAllReportsAsync(bool includeDeleted = false)
        {
            Expression<Func<Report, bool>> filter = r => r.IsDeleted == includeDeleted;

            return await GetAllAsync(
                filter: filter,
                pageIndex: 1,
                pageSize: int.MaxValue,
                includes: new Expression<Func<Report, object>>[]
                {
                r => r.Reporter,
                r => r.Artwork
                });
        }
    }
}
