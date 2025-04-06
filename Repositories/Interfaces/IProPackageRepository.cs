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
    public interface IProPackageRepository : IGenericRepository<ProPackage>
    {
        Task<ProPackage?> GetProPackageByIdWithDetailsAsync(Guid id);
        Task<QueryResultModel<List<ProPackage>>> GetAllAsync(
     Expression<Func<ProPackage, bool>>? filter = null, int pageIndex = 1, int pageSize = 10);
    }
}
