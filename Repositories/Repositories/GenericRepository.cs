using Microsoft.EntityFrameworkCore;
using Repositories.Common;
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
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbSet<TEntity> _dbSet;
        private readonly IClaimsService _claimsService;

        public GenericRepository(AppDbContext dbContext, IClaimsService claimsService)
        {
            _dbSet = dbContext.Set<TEntity>();
            _claimsService = claimsService;
        }

        public virtual async Task<TEntity?> GetAsync(Guid id, string include = "")
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var includeProperty in include.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            var result = await query.FirstOrDefaultAsync(x => x.Id == id);

            // todo: throw exception when result is not found
            return result;
        }

        //public virtual async Task<List<TEntity>> GetAllAsync(string include = "")
        //{
        //    IQueryable<TEntity> query = _dbSet;

        //    foreach (var includeProperty in include.Split
        //                 (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(includeProperty.Trim());
        //    }

        //    return await query.ToListAsync();
        //}

        //public virtual async Task<QueryResultModel<List<TEntity>>> GetAllAsync(
        //    Expression<Func<TEntity, bool>>? filter = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        //    string include = "",
        //    int? pageIndex = null,
        //    int? pageSize = null)
        //{
        //    IQueryable<TEntity> query = _dbSet;

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    int totalCount = await query.CountAsync();

        //    foreach (var includeProperty in include.Split
        //                 (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(includeProperty.Trim());
        //    }

        //    if (orderBy != null)
        //    {
        //        query = orderBy(query);
        //    }

        //    if (pageIndex.HasValue && pageSize.HasValue)
        //    {
        //        int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
        //        int validPageSize =
        //            pageSize.Value > 0
        //                ? pageSize.Value
        //                : PaginationConstant.DEFAULT_MIN_PAGE_SIZE;

        //        query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
        //    }

        //    return new QueryResultModel<List<TEntity>>()
        //    {
        //        TotalCount = totalCount,
        //        Data = await query.ToListAsync(),
        //    };
        //}

        public virtual async Task AddAsync(TEntity entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.CreatedBy = _claimsService.GetCurrentUserId;
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreationDate = DateTime.Now;
                entity.CreatedBy = _claimsService.GetCurrentUserId;
            }

            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(TEntity entity)
        {
            entity.ModificationDate = DateTime.Now;
            entity.ModifiedBy = _claimsService.GetCurrentUserId;
            _dbSet.Update(entity);
        }

        public virtual void UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.ModificationDate = DateTime.Now;
                entity.ModifiedBy = _claimsService.GetCurrentUserId;
            }

            _dbSet.UpdateRange(entities);
        }

        public virtual void SoftDelete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletionDate = DateTime.Now;
            entity.DeletedBy = _claimsService.GetCurrentUserId;
            _dbSet.Update(entity);
        }

        public virtual void SoftDeleteRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletionDate = DateTime.Now;
                entity.DeletedBy = _claimsService.GetCurrentUserId;
            }

            _dbSet.UpdateRange(entities);
        }

        public virtual void Restore(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletionDate = null;
            entity.DeletedBy = null;
            entity.ModificationDate = DateTime.Now;
            entity.ModifiedBy = _claimsService.GetCurrentUserId;
            _dbSet.Update(entity);
        }

        public virtual void RestoreRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = false;
                entity.DeletionDate = null;
                entity.DeletedBy = null;
                entity.ModificationDate = DateTime.Now;
                entity.ModifiedBy = _claimsService.GetCurrentUserId;
            }

            _dbSet.UpdateRange(entities);
        }

        public virtual void HardDelete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void HardDeleteRange(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

    //    public async Task<QueryResultModel<List<TEntity>>> GetAllAsync(
    //Expression<Func<TEntity, bool>>? filter = null,
    //int pageIndex = 1,
    //int pageSize = 10,
    //params Expression<Func<TEntity, object>>[] includes)
    //    {
    //        IQueryable<TEntity> query = _dbSet;

    //        // Áp dụng bộ lọc nếu có
    //        if (filter != null)
    //        {
    //            query = query.Where(filter);
    //        }

    //        // Include các bảng liên quan
    //        foreach (var include in includes)
    //        {
    //            query = query.Include(include);
    //        }

    //        // Kiểm tra nếu có navigation nhiều cấp
    //        if (typeof(TEntity) == typeof(Artwork))
    //        {
    //            query = query.Include("ArtworkCategories.Category");
    //        }

    //        int totalCount = await query.CountAsync();

    //        var data = await query
    //            .Skip((pageIndex - 1) * pageSize)
    //            .Take(pageSize)
    //            .ToListAsync();

    //        return new QueryResultModel<List<TEntity>>(data, totalCount);
    //    }

        public async Task<TEntity?> GetAsync(Guid id, params string[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        //public async Task<List<TEntity>> GetAllAsyncs(string include = "")
        //{
        //    IQueryable<TEntity> query = _dbSet;

        //    if (!string.IsNullOrWhiteSpace(include))
        //    {
        //        foreach (var includeProperty in include.Split(',', StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            query = query.Include(includeProperty);
        //        }
        //    }

        //    return await query.ToListAsync();
        //}

        //public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, params string[] includeProperties)
        //{
        //    IQueryable<TEntity> query = _dbSet;

        //    if (includeProperties != null && includeProperties.Length > 0)
        //    {
        //        foreach (var includeProperty in includeProperties)
        //        {
        //            query = query.Include(includeProperty);
        //        }
        //    }

        //    return await query.FirstOrDefaultAsync(filter);
        //}

        //public async Task<(List<TEntity> Data, int TotalCount)> GetAllAsync(
        //    Expression<Func<TEntity, bool>> filter = null,
        //    int pageIndex = 1,
        //    int pageSize = 10,
        //    params string[] includeProperties)
        //{
        //    IQueryable<TEntity> query = _dbSet;

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    if (includeProperties != null && includeProperties.Length > 0)
        //    {
        //        foreach (var includeProperty in includeProperties)
        //        {
        //            query = query.Include(includeProperty);
        //        }
        //    }

        //    int totalCount = await query.CountAsync();
        //    List<TEntity> data = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        //    return (data, totalCount);
        //}

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet.Where(filter);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync();
        }

      

        public async Task<List<TEntity>> GetAllWithoutPagingAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<QueryResultModel<List<TEntity>>> GetAllWithOrderAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? pageIndex = null, int? pageSize = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            int totalCount = await query.CountAsync();
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return new QueryResultModel<List<TEntity>>
            {
                Data = await query.ToListAsync(),
                TotalCount = totalCount
            };
        }

        public async Task<(List<TEntity> Data, int TotalCount)> GetAllAsync(
    Expression<Func<TEntity, bool>>? filter = null,
    int pageIndex = 1,
    int pageSize = 10,
    params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            // Áp dụng điều kiện lọc (nếu có)
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Bao gồm các thuộc tính liên quan (nếu có)
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Lấy tổng số bản ghi trước khi phân trang
            int totalCount = await query.CountAsync();

            // Phân trang dữ liệu
            List<TEntity> data = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        
    }
}
//using Microsoft.EntityFrameworkCore;
//using Repositories.Common;
//using Repositories.Entities;
//using Repositories.Interfaces;
//using Repositories.Models.QueryModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace Repositories.Repositories
//{
//    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
//    {
//        protected DbSet<TEntity> _dbSet;
//        private readonly IClaimsService _claimsService;

//        public GenericRepository(AppDbContext dbContext, IClaimsService claimsService)
//        {
//            _dbSet = dbContext.Set<TEntity>();
//            _claimsService = claimsService;
//        }

//        public async Task<TEntity?> GetAsync(Guid id, params Expression<Func<TEntity, object>>[] includes)
//        {
//            IQueryable<TEntity> query = _dbSet;

//            foreach (var include in includes)
//            {
//                query = query.Include(include);
//            }

//            return await query.FirstOrDefaultAsync(e => e.Id == id);
//        }

//        public async Task<QueryResultModel<List<TEntity>>> GetAllAsync(
//            Expression<Func<TEntity, bool>>? filter = null,
//            int pageIndex = 1,
//            int pageSize = 10,
//            params Expression<Func<TEntity, object>>[] includes)
//        {
//            IQueryable<TEntity> query = _dbSet;

//            if (filter != null)
//            {
//                query = query.Where(filter);
//            }

//            foreach (var include in includes)
//            {
//                query = query.Include(include);
//            }

//            int totalCount = await query.CountAsync();
//            var data = await query
//                .Skip((pageIndex - 1) * pageSize)
//                .Take(pageSize)
//                .ToListAsync();

//            return new QueryResultModel<List<TEntity>>(data, totalCount);
//        }

//        public async Task AddAsync(TEntity entity)
//        {
//            entity.CreationDate = DateTime.Now;
//            entity.CreatedBy = _claimsService.GetCurrentUserId;
//            await _dbSet.AddAsync(entity);
//        }

//        public async Task AddRangeAsync(List<TEntity> entities)
//        {
//            foreach (var entity in entities)
//            {
//                entity.CreationDate = DateTime.Now;
//                entity.CreatedBy = _claimsService.GetCurrentUserId;
//            }

//            await _dbSet.AddRangeAsync(entities);
//        }

//        public void Update(TEntity entity)
//        {
//            entity.ModificationDate = DateTime.Now;
//            entity.ModifiedBy = _claimsService.GetCurrentUserId;
//            _dbSet.Update(entity);
//        }

//        public void UpdateRange(List<TEntity> entities)
//        {
//            foreach (var entity in entities)
//            {
//                entity.ModificationDate = DateTime.Now;
//                entity.ModifiedBy = _claimsService.GetCurrentUserId;
//            }

//            _dbSet.UpdateRange(entities);
//        }

//        public void SoftDelete(TEntity entity)
//        {
//            entity.IsDeleted = true;
//            entity.DeletionDate = DateTime.Now;
//            entity.DeletedBy = _claimsService.GetCurrentUserId;
//            _dbSet.Update(entity);
//        }

//        public void SoftDeleteRange(List<TEntity> entities)
//        {
//            foreach (var entity in entities)
//            {
//                entity.IsDeleted = true;
//                entity.DeletionDate = DateTime.Now;
//                entity.DeletedBy = _claimsService.GetCurrentUserId;
//            }

//            _dbSet.UpdateRange(entities);
//        }

//        public void Restore(TEntity entity)
//        {
//            entity.IsDeleted = false;
//            entity.DeletionDate = null;
//            entity.DeletedBy = null;
//            entity.ModificationDate = DateTime.Now;
//            entity.ModifiedBy = _claimsService.GetCurrentUserId;
//            _dbSet.Update(entity);
//        }

//        public void RestoreRange(List<TEntity> entities)
//        {
//            foreach (var entity in entities)
//            {
//                entity.IsDeleted = false;
//                entity.DeletionDate = null;
//                entity.DeletedBy = null;
//                entity.ModificationDate = DateTime.Now;
//                entity.ModifiedBy = _claimsService.GetCurrentUserId;
//            }

//            _dbSet.UpdateRange(entities);
//        }

//        public void HardDelete(TEntity entity)
//        {
//            _dbSet.Remove(entity);
//        }

//        public void HardDeleteRange(List<TEntity> entities)
//        {
//            _dbSet.RemoveRange(entities);
//        }

//        public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
//        {
//            IQueryable<TEntity> query = _dbSet;

//            // Include các navigation properties
//            foreach (var include in includes)
//            {
//                query = query.Include(include);
//            }

//            return await query.FirstOrDefaultAsync(filter);
//        }

//    }
//}

