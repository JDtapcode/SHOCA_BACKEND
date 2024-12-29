using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class AccountProPackage:BaseEntity
    {
        public Guid AccountId { get; set; }
        public Guid ProPackageId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PackageStatus PackageStatus { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ProPackage? ProPackage { get; set; }
    }

}
