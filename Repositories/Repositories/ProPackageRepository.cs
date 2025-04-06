using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Models.ProPackageModels;
using Repositories.Models.ProPackages;
using Repositories.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ProPackageRepository : GenericRepository<ProPackage>, IProPackageRepository
    {
        public ProPackageRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
        }

        public async Task<QueryResultModel<List<ProPackage>>> GetAllAsync(
     Expression<Func<ProPackage, bool>>? filter = null, int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<ProPackage> query = _dbSet
                .Include(p => p.Features) // ✅ Load Features để lấy danh sách tính năng của gói
                .AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            int totalCount = await query.CountAsync();
            var data = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new QueryResultModel<List<ProPackage>>(data, totalCount);
        }

        public async Task<ProPackage?> GetProPackageByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.Features)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
