using Microsoft.AspNetCore.Identity;
using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//test

namespace Repositories.Entities
{
    public class Account:IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsMembership { get; set; }
        public string? PersonalWebsiteUrl {  get; set; }
        public string? PortfolioUrl {  get; set; }
        // Refresh Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? VerificationCode { get; set; }
        public DateTime? VerificationCodeExpiryTime { get; set; }
        // Base Entity
        // Note: This class cannot inherit from 2 classes (BaseEntity, IdentityUser) at the same 
        public DateTime CreationDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<AccountProPackage> AccountProPackages { get; set; }= new List<AccountProPackage>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Job> Jobs { get; set; }= new List<Job>();
        public virtual ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
        public virtual ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
        public virtual ICollection<Rating> Ratings { get; set; }= new List<Rating>();
        public virtual ICollection<FreelancerService> FreelancerServices { get; set; } = new List<FreelancerService>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
