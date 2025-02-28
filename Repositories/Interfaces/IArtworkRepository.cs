using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IArtworkRepository : IGenericRepository<Artwork>
    {
        Task<Artwork?> GetArtworkByIdWithDetailsAsync(Guid id);
    }
}
