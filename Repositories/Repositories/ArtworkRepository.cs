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
    public class ArtworkRepository : GenericRepository<Artwork>, IArtworkRepository
    {
        private readonly AppDbContext _dbContext;
        public ArtworkRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<Artwork?> GetArtworkByIdWithDetailsAsync(Guid id)
        {
            return await _dbContext.Artworks
                .Include(a => a.ArtworkCategories)
                    .ThenInclude(ac => ac.Category)  
                .Include(a => a.Images)              
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Artwork?> GetArtworkWithImagesAsync(Guid id)
        {
            return await _dbContext.Artworks
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
