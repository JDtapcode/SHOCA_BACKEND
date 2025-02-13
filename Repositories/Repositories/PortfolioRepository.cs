using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class PortfolioRepository : GenericRepository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
        }
    }
}
