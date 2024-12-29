using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Transaction:BaseEntity
    {
        public decimal MoneyAmount { get; set; }
        public Guid? ArtworkId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public Guid? ProPackageId { get; set; }

        public virtual Artwork? Artwork { get; set; }
        public virtual Account? User { get; set; }
        public virtual ProPackage? ProPackage { get; set; }
    }

}
