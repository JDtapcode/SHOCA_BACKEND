using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class PortfolioRepository : GenericRepository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
        }
        public async Task<Portfolio?> GetPortfolioByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.PortfolioImages)
                .ThenInclude(pi => pi.ArtworkImage)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<QueryResultModel<List<Portfolio>>> GetAllWithDetailsAsync(Expression<Func<Portfolio, bool>>? filter = null, int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<Portfolio> query = _dbSet
                .Include(p => p.PortfolioImages)
                .ThenInclude(pi => pi.ArtworkImage);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            int totalCount = await query.CountAsync();
            var data = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new QueryResultModel<List<Portfolio>>(data, totalCount);
        }
    }
}
