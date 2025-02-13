using Repositories.Models.RatingModels;
using Services.Common;
using Services.Models.RatingModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRatingService
    {
        Task<ResponseModel> CreateRating(RatingCreateModel ratingCreateModel);
        Task<ResponseModel> AddComment(RatingCommentCreateModel ratingCommentCreateModel);
        Task<Pagination<RatingModel>> GetRatingsByArtworkAsync(RatingFilterModel model);
        Task<ResponseDataModel<RatingModel>> GetRatingById(Guid ratingId);
        Task<ResponseModel> HardDeleteRatingAsync(Guid ratingId);
    }
}
