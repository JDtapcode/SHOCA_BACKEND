﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IFreelancerServiceRepository FreelancerServiceRepository { get; }
        IJobRepository JobRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IArtworkRepository ArtworkRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
