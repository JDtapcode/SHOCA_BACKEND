using Repositories.Entities;
using Repositories.Enums;
using Repositories.Models.AccountProPackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.AccountModels
{
    public class AccountModel : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string? PersonalWebsiteUrl { get; set; }
        public string? PortfolioUrl { get; set; }
        public string? Role { get; set; }
        public List<AccountProPackageInfo> PurchasedPackages { get; set; } = new List<AccountProPackageInfo>();
    }
}
