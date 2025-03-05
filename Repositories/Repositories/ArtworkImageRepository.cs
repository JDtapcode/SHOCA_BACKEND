using Microsoft.EntityFrameworkCore;
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
        private readonly DbContext _dbContext;
        public ArtworkImageRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ArtworkImage>> GetByCreatedByAsync(Guid createdBy)
        {
            return await _dbContext.Set<ArtworkImage>()
                .Where(img => img.CreatedBy == createdBy && !img.IsDeleted)
                .ToListAsync();
        }
    }
}
