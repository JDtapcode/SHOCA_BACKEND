using Microsoft.EntityFrameworkCore;
using Repositories.Common;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Models.AccountModels;
using Repositories.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _dbContext;

        public AccountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<QueryResultModel<List<AccountModel>>> GetAllAsync(
            Expression<Func<AccountModel, bool>>? filter = null,
            Func<IQueryable<AccountModel>, IOrderedQueryable<AccountModel>>? orderBy = null,
            string include = "",
            int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<AccountModel> query = _dbContext.Users
                .Join(
                    _dbContext.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new { user, userRole }
                )
                .Join(
                    _dbContext.Roles,
                    userRolePair => userRolePair.userRole.RoleId,
                    role => role.Id,
                    (userRolePair, role) => new AccountModel()
                    {
                        Id = userRolePair.user.Id,
                        FirstName = userRolePair.user.FirstName,
                        LastName = userRolePair.user.LastName,
                        Gender = userRolePair.user.Gender,
                        DateOfBirth = userRolePair.user.DateOfBirth,
                        Address = userRolePair.user.Address,
                        AvatarUrl = userRolePair.user.AvatarUrl,
                        Email = userRolePair.user.Email,
                        EmailConfirmed = userRolePair.user.EmailConfirmed,
                        PhoneNumber = userRolePair.user.PhoneNumber,
                        Role = role.Name!,
                        PersonalWebsiteUrl= userRolePair.user.PersonalWebsiteUrl,
                        PortfolioUrl= userRolePair.user.PortfolioUrl,
                        CreationDate = userRolePair.user.CreationDate,
                        CreatedBy = userRolePair.user.CreatedBy,
                        ModificationDate = userRolePair.user.ModificationDate,
                        ModifiedBy = userRolePair.user.ModifiedBy,
                        DeletionDate = userRolePair.user.DeletionDate,
                        DeletedBy = userRolePair.user.DeletedBy,
                        IsDeleted = userRolePair.user.IsDeleted,
                    }
                );

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

            return new QueryResultModel<List<AccountModel>>()
            {
                TotalCount = totalCount,
                Data = await query.ToListAsync(),
            };
        }

        public async Task<Account> GetAsync(Guid id)
        {
            return await _dbContext.Accounts.FindAsync(id);
        }
    }
}
