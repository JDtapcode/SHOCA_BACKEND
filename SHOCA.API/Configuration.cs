using Microsoft.AspNetCore.Identity;
using Repositories.Entities;
using Repositories;
using SHOCA.API.Middlewares;
using System.Diagnostics;
using Services.Common;
using SHOCA.API.Utils;
using Repositories.Interfaces;
using Repositories.Common;
using Services.Interfaces;
using Services.Services;
using Repositories.Repositories;

namespace SHOCA.API
{
    public static class Configuration
    {
        public static IServiceCollection AddAPIConfiguration(this IServiceCollection services)
        {
            // Identity
            services
                .AddIdentity<Account, Role>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;
                })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(15);
            });

            // Middlewares
            services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddSingleton<PerformanceMiddleware>();
            services.AddScoped<AccountStatusMiddleware>();
            services.AddSingleton<Stopwatch>();

            // Common
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
            services.AddScoped<IClaimsService,ClaimsService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailService, EmailService>();

            // Dependency Injection
            // Account
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            //FreelancerService
            services.AddScoped<IFreelancerServiceService, FreelancerServiceService>();
            services.AddScoped<IFreelancerServiceRepository,FreelancerServiceRepository>();
            

            //Job
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IJobRepository, JobRepository>();

            //Category
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            //Artwork
            services.AddScoped<IArtworkService, ArtworkService>();
            services.AddScoped<IArtworkRepository, ArtworkRepository>();

            //Rating
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IRatingCommentRepository, RatingCommentRepository>();

            //ProPackage
            services.AddScoped<IProPackageRepository, ProPackageRepository>();
            services.AddScoped<IProPackageService, ProPackageService>();

            //Portfolio
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IPortfolioImageRepository, PortfolioImageRepository>();
            //Transaction
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            //AccountProPackage
            services.AddScoped<IAccountProPackageRepository,AcccountProPackageRepository>();

            //Image
            services.AddScoped<IArtworkImageRepository, ArtworkImageRepository>();
            services.AddScoped<IArtworkCategoryRepository, ArtworkCategoryRepository>();
            return services;
        }
        }
}
