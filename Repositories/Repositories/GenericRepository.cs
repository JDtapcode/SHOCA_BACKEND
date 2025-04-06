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
        private readonly AppDbContext _context;
        protected DbSet<TEntity> _dbSet;
        private readonly IClaimsService _claimsService;

        public GenericRepository(AppDbContext dbContext, IClaimsService claimsService)
        {
            _context = dbContext;
            _dbSet = dbContext.Set<TEntity>();
            _claimsService = claimsService;
        }
        public async Task<(List<TEntity> Data, int TotalCount)> GetAllAsyncs(
    Expression<Func<TEntity, bool>>? filter = null,
    int pageIndex = 1,
    int pageSize = 10,
    params Expression<Func<TEntity, object>>[] includes)
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

            if (typeof(TEntity) == typeof(Artwork))
            {
                query = query.Include("ArtworkCategories.Category");
            }

            int totalCount = await query.CountAsync();

            List<TEntity> data = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
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
        public async Task<TEntity?> GetAsync(Guid id, params string[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }
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

