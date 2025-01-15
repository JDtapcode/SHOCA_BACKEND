using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IFreelancerServiceRepository _freelancerServiceRepository;
        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, IFreelancerServiceRepository freelancerServiceRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _freelancerServiceRepository = freelancerServiceRepository;
        }
        public AppDbContext DbContext => _dbContext;

        public IAccountRepository AccountRepository => _accountRepository;
        public IFreelancerServiceRepository FreelancerServiceRepository => _freelancerServiceRepository;
        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
