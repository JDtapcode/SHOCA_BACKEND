using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
        }

        public async Task<Transaction> GetByOrderCodeAsync(string orderCode)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.OrderCode == orderCode);
        }
    }
}
