//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using Repositories.Entities;
//using System;

//namespace Repositories.Data
//{
//    public class AppDbContext : IdentityDbContext<Account, Role, Guid>
//    {
//        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//        // DbSet for each entity
//        public DbSet<Account> Accounts { get; set; }
//        public DbSet<AccountProPackage> AccountProPackages { get; set; }
//        public DbSet<Artwork> Artworks { get; set; }
//        public DbSet<ArtworkCategory> ArtworkCategories { get; set; }
//        public DbSet<Category> Categories { get; set; }
//        public DbSet<FreelancerService> FreelancerServices { get; set; }
//        public DbSet<Job> Jobs { get; set; }
//        public DbSet<Portfolio> Portfolios { get; set; }
//        public DbSet<ProPackage> ProPackages { get; set; }
//        public DbSet<Rating> Ratings { get; set; }
//        public DbSet<RatingComment> RatingComments { get; set; }
//        public DbSet<Transaction> Transactions { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // Account relationships
//            modelBuilder.Entity<Account>()
//                .HasMany(a => a.AccountProPackages)
//                .WithOne(ap => ap.Account)
//                .HasForeignKey(ap => ap.AccountId);

//            modelBuilder.Entity<Account>()
//                .HasMany(a => a.Transactions)
//                .WithOne(t => t.User)
//                .HasForeignKey(t => t.UserId);

//            modelBuilder.Entity<Account>()
//                .HasMany(a => a.Artworks)
//                .WithOne(a => a.Creator)
//                .HasForeignKey(a => a.CreatorId);

//            modelBuilder.Entity<Account>()
//                .HasMany(a => a.Portfolios)
//                .WithOne(p => p.User)
//                .HasForeignKey(p => p.UserId);

//            modelBuilder.Entity<Account>()
//                .HasMany(a => a.Ratings)
//                .WithOne(r => r.Customer)
//                .HasForeignKey(r => r.CustomerId);

//            modelBuilder.Entity<Account>()
//                .HasMany(a => a.FreelancerServices)
//                .WithOne(fs => fs.User)
//                .HasForeignKey(fs => fs.UserId);

//            // Artwork relationships
//            modelBuilder.Entity<Artwork>()
//                .HasMany(a => a.Transactions)
//                .WithOne(t => t.Artwork)
//                .HasForeignKey(t => t.ArtworkId);

//            modelBuilder.Entity<Artwork>()
//                .HasMany(a => a.Ratings)
//                .WithOne(r => r.Artwork)
//                .HasForeignKey(r => r.ArtworkId);

//            modelBuilder.Entity<Artwork>()
//                .HasMany(a => a.ArtworkCategories)
//                .WithOne(ac => ac.Portfolio)
//                .HasForeignKey(ac => ac.PortfolioId);

//            // Portfolio relationships
//            modelBuilder.Entity<Portfolio>()
//                .HasMany(p => p.Artworks)
//                .WithOne(a => a.Portfolio)
//                .HasForeignKey(a => a.PortfolioId);

//            modelBuilder.Entity<Portfolio>()
//                .HasMany(p => p.ArtworkCategories)
//                .WithOne(ac => ac.Portfolio)
//                .HasForeignKey(ac => ac.PortfolioId);

//            // RatingComment relationships
//            modelBuilder.Entity<RatingComment>()
//                .HasMany(rc => rc.ChildComments)
//                .WithOne(cc => cc.ParentComment)
//                .HasForeignKey(cc => cc.ParentCommentId);

//            modelBuilder.Entity<RatingComment>()
//                .HasOne(rc => rc.Rating)
//                .WithMany(r => r.CommentsList)
//                .HasForeignKey(rc => rc.RatingId);
//        }
//    }
//}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using System;

namespace Repositories
{
    public class AppDbContext : IdentityDbContext<Account, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<ProPackage> ProPackages { get; set; }
        public DbSet<AccountProPackage> AccountProPackages { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArtworkCategory> ArtworkCategories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<FreelancerService> FreelancerServices { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình bảng Account
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(x => x.FirstName).HasMaxLength(50);
                entity.Property(x => x.LastName).HasMaxLength(50);
                entity.Property(x => x.PhoneNumber).HasMaxLength(15);
                entity.Property(x => x.VerificationCode).HasMaxLength(6);
            });

            // Cấu hình bảng Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(x => x.Description).HasMaxLength(256);
            });
            modelBuilder.Entity<Artwork>(entity =>
            {
                entity.HasOne(a => a.Creator)
                    .WithMany(c => c.Artworks)
                    .HasForeignKey(a => a.CreatorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Portfolio)
                    .WithMany(p => p.Artworks)
                    .HasForeignKey(a => a.PortfolioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<ArtworkCategory>(entity =>
            {
                entity.HasKey(ac => new { ac.ArtworkId, ac.CategoryId });

                entity.HasOne(ac => ac.Artwork)
                    .WithMany(a => a.ArtworkCategories)
                    .HasForeignKey(ac => ac.ArtworkId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ac => ac.Category)
                    .WithMany(c => c.ArtworkCategories)
                    .HasForeignKey(ac => ac.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<AccountProPackage>()
    .HasOne(ap => ap.Account)
    .WithMany(a => a.AccountProPackages)
    .HasForeignKey(ap => ap.AccountId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountProPackage>()
                .HasOne(ap => ap.ProPackage)
                .WithMany(pp => pp.AccountProPackages)
                .HasForeignKey(ap => ap.ProPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Artwork)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.ArtworkId)
                .OnDelete(DeleteBehavior.Restrict);


            // Cấu hình bảng Rating
            modelBuilder.Entity<Rating>()
    .HasOne(r => r.Customer)
    .WithMany(a => a.Ratings)
    .HasForeignKey(r => r.CustomerId)
    .OnDelete(DeleteBehavior.Restrict); // Thêm để kiểm soát hành vi xóa.

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Artwork)
                .WithMany(a => a.Ratings)
                .HasForeignKey(r => r.ArtworkId)
                .OnDelete(DeleteBehavior.Restrict); // Thêm để kiểm soát hành vi xóa.


        }
    }
}
