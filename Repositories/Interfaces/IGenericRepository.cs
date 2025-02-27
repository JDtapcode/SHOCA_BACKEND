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
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetAsync(Guid id, string include = "");
        Task<TEntity?> GetAsync(Guid id, params string[] includes);
        //Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, params string[] includeProperties);
        //    Task<(List<TEntity> Data, int TotalCount)> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null,
        //                                                      int pageIndex = 1, int pageSize = 10,
        //                                                      params string[] includeProperties);

        //    Task<List<TEntity>> GetAllAsyncs(string include = "");
        //    Task<QueryResultModel<List<TEntity>>> GetAllAsync(
        //Expression<Func<TEntity, bool>>? filter = null,
        //int pageIndex = 1,
        //int pageSize = 10,
        //params Expression<Func<TEntity, object>>[] includes);


        //    Task<QueryResultModel<List<TEntity>>> GetAllAsync(
        //        Expression<Func<TEntity, bool>>? filter = null,
        //        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        //        string include = "",
        //        int? pageIndex = null,
        //        int? pageSize = null
        //    );
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);

        // Lấy danh sách entity có phân trang
        Task<(List<TEntity> Data, int TotalCount)> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            int pageIndex = 1,
            int pageSize = 10,
            params Expression<Func<TEntity, object>>[] includes);

        Task<List<TEntity>> GetAllWithoutPagingAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes);

        Task<QueryResultModel<List<TEntity>>> GetAllWithOrderAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? pageIndex = null,
            int? pageSize = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void SoftDelete(TEntity entity);
        void SoftDeleteRange(List<TEntity> entities);
        void Restore(TEntity entity);
        void RestoreRange(List<TEntity> entities);
        void HardDelete(TEntity entity);
        void HardDeleteRange(List<TEntity> entities);
    }
}
//using Repositories.Entities;
//using Repositories.Models.QueryModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace Repositories.Interfaces
//{
//    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
//    {
//        Task<TEntity?> GetAsync(Guid id, params Expression<Func<TEntity, object>>[] includes);
//        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);

//        Task<QueryResultModel<List<TEntity>>> GetAllAsync(
//            Expression<Func<TEntity, bool>>? filter = null,
//            int pageIndex = 1,
//            int pageSize = 10,
//            params Expression<Func<TEntity, object>>[] includes
//        );

//        Task AddAsync(TEntity entity);
//        Task AddRangeAsync(List<TEntity> entities);
//        void Update(TEntity entity);
//        void UpdateRange(List<TEntity> entities);
//        void SoftDelete(TEntity entity);
//        void SoftDeleteRange(List<TEntity> entities);
//        void Restore(TEntity entity);
//        void RestoreRange(List<TEntity> entities);
//        void HardDelete(TEntity entity);
//        void HardDeleteRange(List<TEntity> entities);
//    }
//}
