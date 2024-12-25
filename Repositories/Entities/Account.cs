using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Account:IdentityUser<Guid>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Bio { get; set; }
        public string AvatarUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int RoleId { get; set; }
        public bool IsMembership { get; set; }
        public string Skills { get; set; }
        public string Password { get; set; }
        //Note : Can't inherit BaseEntity and identityUser at the same time
        public DateTime CreationDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;

        //public ICollection<Portfolio> Portfolios { get; set; }
        //public ICollection<Transaction> Transactions { get; set; }
        //public ICollection<Job> Jobs { get; set; }
        //public ICollection<FreelancerService> FreelancerServices { get; set; }
        //public ICollection<Rating> Ratings { get; set; }
    }
}
