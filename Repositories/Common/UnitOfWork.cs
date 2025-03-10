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
        private readonly IJobRepository _jobRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArtworkRepository _artworkRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IRatingCommentRepository _ratingCommentRepository;
        private readonly IProPackageRepository _proPackageRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IAccountProPackageRepository _accountProPackageRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IArtworkImageRepository _artworkImageRepository;
        private readonly IPortfolioImageRepository _portfolioImageRepository;
        private readonly IArtworkCategoryRepository _artworkCategoryRepository;
        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, IFreelancerServiceRepository freelancerServiceRepository,IJobRepository jobRepository, ICategoryRepository categoryRepository,IArtworkRepository artworkRepository,IRatingRepository ratingRepository,IRatingCommentRepository ratingCommentRepository,IProPackageRepository proPackageRepository,IPortfolioRepository portfolioRepository,IAccountProPackageRepository accountProPackageRepository,ITransactionRepository transactionRepository,IArtworkImageRepository artworkImageRepository,IPortfolioImageRepository portfolioImageRepository,IArtworkCategoryRepository artworkCategoryRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _freelancerServiceRepository = freelancerServiceRepository;
            _jobRepository = jobRepository;
            _categoryRepository = categoryRepository;
            _artworkRepository = artworkRepository;
            _ratingRepository = ratingRepository;
            _ratingCommentRepository = ratingCommentRepository;
            _proPackageRepository = proPackageRepository;
            _portfolioRepository = portfolioRepository;
            _transactionRepository = transactionRepository;
            _accountProPackageRepository = accountProPackageRepository;
            _artworkImageRepository = artworkImageRepository;
            _portfolioImageRepository=portfolioImageRepository;
            _artworkCategoryRepository = artworkCategoryRepository;
        }
        public AppDbContext DbContext => _dbContext;

        public IAccountRepository AccountRepository => _accountRepository;
        public IFreelancerServiceRepository FreelancerServiceRepository => _freelancerServiceRepository;
        public IJobRepository JobRepository => _jobRepository;

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public IArtworkRepository ArtworkRepository => _artworkRepository;

        public IRatingRepository RatingRepository => _ratingRepository;

        public IRatingCommentRepository CommentRepository => _ratingCommentRepository;

        public IProPackageRepository ProPackageRepository => _proPackageRepository;

        public IPortfolioRepository PortfolioRepository => _portfolioRepository;

        public ITransactionRepository TransactionRepository => _transactionRepository;

        public IAccountProPackageRepository AccountProPackageRepository => _accountProPackageRepository;

        public IArtworkImageRepository ArtworkImageRepository => _artworkImageRepository;

        public IPortfolioImageRepository PortfolioImageRepository => _portfolioImageRepository;
        public IArtworkCategoryRepository ArtworkCategoryRepository => _artworkCategoryRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
