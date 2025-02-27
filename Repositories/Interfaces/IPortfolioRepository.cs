using Repositories.Entities;
using Repositories.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IPortfolioRepository:IGenericRepository<Portfolio>
    {
        Task<Portfolio?> GetPortfolioByIdWithDetailsAsync(Guid id);
        Task<QueryResultModel<List<Portfolio>>> GetAllWithDetailsAsync(Expression<Func<Portfolio, bool>>? filter = null, int pageIndex = 1, int pageSize = 10);
    }
}
