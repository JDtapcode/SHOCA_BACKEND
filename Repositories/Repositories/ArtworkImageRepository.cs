using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ArtworkImageRepository : GenericRepository<ArtworkImage>, IArtworkImageRepository
    {
        public ArtworkImageRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
        }
    }
}
