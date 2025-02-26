using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Enums;
using System;

namespace Repositories
{
    //public class AppDbContext : IdentityDbContext<Account, Role, Guid>
    //{
    //    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    //    {
    //    }

    //    public DbSet<Account> Accounts { get; set; }
    //    public DbSet<ProPackage> ProPackages { get; set; }
    //    public DbSet<AccountProPackage> AccountProPackages { get; set; }
    //    public DbSet<Transaction> Transactions { get; set; }
    //    public DbSet<Artwork> Artworks { get; set; }
    //    public DbSet<Category> Categories { get; set; }
    //    public DbSet<ArtworkCategory> ArtworkCategories { get; set; }
    //    public DbSet<Job> Jobs { get; set; }
    //    public DbSet<FreelancerService> FreelancerServices { get; set; }
    //    public DbSet<Portfolio> Portfolios { get; set; }
    //    public DbSet<Rating> Ratings { get; set; }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);

    //        // Cấu hình bảng Account
    //        modelBuilder.Entity<Account>(entity =>
    //        {
    //            entity.Property(x => x.FirstName).HasMaxLength(50);
    //            entity.Property(x => x.LastName).HasMaxLength(50);
    //            entity.Property(x => x.PhoneNumber).HasMaxLength(15);
    //            entity.Property(x => x.VerificationCode).HasMaxLength(6);
    //        });

    //        // Cấu hình bảng Role
    //        modelBuilder.Entity<Role>(entity =>
    //        {
    //            entity.Property(x => x.Description).HasMaxLength(256);
    //        });
    //        modelBuilder.Entity<Artwork>(entity =>
    //        {
    //            entity.HasOne(a => a.Creator)
    //                .WithMany(c => c.Artworks)
    //                .HasForeignKey(a => a.CreatorId)
    //                .OnDelete(DeleteBehavior.Restrict);

    //            entity.HasOne(a => a.Portfolio)
    //                .WithMany(p => p.Artworks)
    //                .HasForeignKey(a => a.PortfolioId)
    //                .OnDelete(DeleteBehavior.Restrict);
    //        });
    //        modelBuilder.Entity<ArtworkCategory>(entity =>
    //        {
    //            entity.HasKey(ac => new { ac.ArtworkId, ac.CategoryId });

    //            entity.HasOne(ac => ac.Artwork)
    //                .WithMany(a => a.ArtworkCategories)
    //                .HasForeignKey(ac => ac.ArtworkId)
    //                .OnDelete(DeleteBehavior.Cascade);

    //            entity.HasOne(ac => ac.Category)
    //                .WithMany(c => c.ArtworkCategories)
    //                .HasForeignKey(ac => ac.CategoryId)
    //                .OnDelete(DeleteBehavior.Cascade);
    //        });


    //        modelBuilder.Entity<AccountProPackage>()
    //.HasOne(ap => ap.Account)
    //.WithMany(a => a.AccountProPackages)
    //.HasForeignKey(ap => ap.AccountId)
    //.OnDelete(DeleteBehavior.Restrict);

    //        modelBuilder.Entity<AccountProPackage>()
    //            .HasOne(ap => ap.ProPackage)
    //            .WithMany(pp => pp.AccountProPackages)
    //            .HasForeignKey(ap => ap.ProPackageId)
    //            .OnDelete(DeleteBehavior.Restrict);

    //        modelBuilder.Entity<Transaction>()
    //            .HasOne(t => t.User)
    //            .WithMany(a => a.Transactions)
    //            .HasForeignKey(t => t.UserId)
    //            .OnDelete(DeleteBehavior.Restrict);

    //        modelBuilder.Entity<Transaction>()
    //            .HasOne(t => t.Artwork)
    //            .WithMany(a => a.Transactions)
    //            .HasForeignKey(t => t.ArtworkId)
    //            .OnDelete(DeleteBehavior.Restrict);


    //        // Cấu hình bảng Rating
    //        modelBuilder.Entity<Rating>()
    //.HasOne(r => r.Customer)
    //.WithMany(a => a.Ratings)
    //.HasForeignKey(r => r.CustomerId)
    //.OnDelete(DeleteBehavior.Restrict); // Thêm để kiểm soát hành vi xóa.

    //        modelBuilder.Entity<Rating>()
    //            .HasOne(r => r.Artwork)
    //            .WithMany(a => a.Ratings)
    //            .HasForeignKey(r => r.ArtworkId)
    //            .OnDelete(DeleteBehavior.Restrict); // Thêm để kiểm soát hành vi xóa.


    //    }
    //}
    public class AppDbContext : IdentityDbContext<Account, Entities.Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<ProPackage> ProPackages { get; set; }
        public DbSet<AccountProPackage> AccountProPackages { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<ArtworkImage> ArtworkImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArtworkCategory> ArtworkCategories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<FreelancerService> FreelancerServices { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PortfolioImage> PortfolioImages { get; set; }
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
            modelBuilder.Entity<Entities.Role>(entity =>
            {
                entity.Property(x => x.Description).HasMaxLength(256);
            });

            // Cấu hình Artwork
            modelBuilder.Entity<Artwork>(entity =>
            {
                entity.HasOne(a => a.Creator)
                    .WithMany(c => c.Artworks)
                    .HasForeignKey(a => a.CreatorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(a => a.Status)
                    .HasDefaultValue(ArtworkStatus.Pending);
            });

            // ArtworkImage (1-N: Artwork -> ArtworkImages)
            modelBuilder.Entity<ArtworkImage>(entity =>
            {
                entity.HasOne(ai => ai.Artwork)
                    .WithMany(a => a.Images)
                    .HasForeignKey(ai => ai.ArtworkId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // PortfolioImage (N-N: Portfolio - ArtworkImage)
            modelBuilder.Entity<PortfolioImage>(entity =>
            {
                entity.HasKey(pi => new { pi.PortfolioId, pi.ArtworkImageId });

                entity.HasOne(pi => pi.Portfolio)
                    .WithMany(p => p.PortfolioImages)
                    .HasForeignKey(pi => pi.PortfolioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pi => pi.ArtworkImage)
                    .WithMany(ai => ai.PortfolioImages)
                    .HasForeignKey(pi => pi.ArtworkImageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình bảng ArtworkCategory (N-N: Artwork - Category)
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

            // Cấu hình AccountProPackage (N-N: Account - ProPackage)
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

            // Cấu hình Transaction (1-N: Account -> Transaction, 1-N: Artwork -> Transaction)
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

            // Cấu hình bảng Rating (1-N: Account -> Rating, 1-N: Artwork -> Rating)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Customer)
                .WithMany(a => a.Ratings)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Artwork)
                .WithMany(a => a.Ratings)
                .HasForeignKey(r => r.ArtworkId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
