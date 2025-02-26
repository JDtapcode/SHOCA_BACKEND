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

        public virtual async Task<List<TEntity>> GetAllAsync(string include = "")
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var includeProperty in include.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            return await query.ToListAsync();
        }

        public virtual async Task<QueryResultModel<List<TEntity>>> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string include = "",
            int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            int totalCount = await query.CountAsync();

            foreach (var includeProperty in include.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize =
                    pageSize.Value > 0
                        ? pageSize.Value
                        : PaginationConstant.DEFAULT_MIN_PAGE_SIZE;

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return new QueryResultModel<List<TEntity>>()
            {
                TotalCount = totalCount,
                Data = await query.ToListAsync(),
            };
        }

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

        public Task<List<TEntity>> GetAllAsyncs(string include = "")
        {
            throw new NotImplementedException();
        }

        public async Task<QueryResultModel<List<TEntity>>> GetAllAsync(
    Expression<Func<TEntity, bool>>? filter = null,
    int pageIndex = 1,
    int pageSize = 10,
    params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            // Áp dụng bộ lọc nếu có
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include các bảng liên quan
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Kiểm tra nếu có navigation nhiều cấp
            if (typeof(TEntity) == typeof(Artwork))
            {
                query = query.Include("ArtworkCategories.Category"); 
            }

            int totalCount = await query.CountAsync();

            var data = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new QueryResultModel<List<TEntity>>(data, totalCount);
        }
       
        public async Task<TEntity?> GetAsync(Guid id, params string[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include); 
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }








    }
}
