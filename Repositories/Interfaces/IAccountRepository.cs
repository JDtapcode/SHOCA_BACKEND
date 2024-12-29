using Repositories.Models.AccountModels;
using Repositories.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<QueryResultModel<List<AccountModel>>> GetAllAsync(
            Expression<Func<AccountModel, bool>>? filter = null,
            Func<IQueryable<AccountModel>, IOrderedQueryable<AccountModel>>? orderBy = null,
            string include = "",
            int? pageIndex = null,
            int? pageSize = null
        );
    }
}
