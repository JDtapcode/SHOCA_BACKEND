using AutoMapper;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Models.ArtworkModels;
using Services.Common;
using Services.Interfaces;
using Services.Models.ArtworkModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArtworkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateArtworkAsync(ArtworkCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                return new ResponseModel { Status = false, Message = "Invalid input data" };
            }

            var artwork = _mapper.Map<Artwork>(model);
            await _unitOfWork.ArtworkRepository.AddAsync(artwork);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Artwork created successfully" };
        }

        public async Task<ResponseModel> DeleteArtworkAsync(Guid id)
        {
            var artwork = await _unitOfWork.ArtworkRepository.GetAsync(id);
            if (artwork == null) return new ResponseModel { Status = false, Message = "Artwork not found" };

            _unitOfWork.ArtworkRepository.SoftDelete(artwork);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Artwork deleted successfully" };
        }

        public async Task<Pagination<ArtworkModel>> GetAllArtworkAsync(ArtworkFilterModel filterModel)
        {
            var queryResult = await _unitOfWork.ArtworkRepository.GetAllAsync(
    filter: a => (a.IsDeleted == filterModel.isDeleted) &&
                 (filterModel.MinPrice == null || a.Price >= filterModel.MinPrice) &&
                 (filterModel.MaxPrice == null || a.Price <= filterModel.MaxPrice) &&
                 (filterModel.CategoryId == null || a.ArtworkCategories.Any(ac => ac.CategoryId == filterModel.CategoryId)) &&
                 (filterModel.CreatorId == null || a.CreatorId == filterModel.CreatorId),
    pageIndex: filterModel.PageIndex,
    pageSize: filterModel.PageSize
);


            var artworks = _mapper.Map<List<ArtworkModel>>(queryResult.Data);
            return new Pagination<ArtworkModel>(artworks, filterModel.PageIndex, filterModel.PageSize, queryResult.TotalCount);
        }

        public async Task<ResponseDataModel<ArtworkModel>> GetArtworkByIdAsync(Guid id)
        {
            var artwork = await _unitOfWork.ArtworkRepository.GetAsync(id);
            if (artwork == null || artwork.IsDeleted)
            {
                return new ResponseDataModel<ArtworkModel> { Status = false, Message = "Artwork not found" };
            }

            var artworkModel = _mapper.Map<ArtworkModel>(artwork);
            return new ResponseDataModel<ArtworkModel> { Status = true, Data = artworkModel };
        }
        public async Task<ResponseModel> RestoreArtwork(Guid id)
        {
            var artwork = await _unitOfWork.ArtworkRepository.GetAsync(id);
            if (artwork == null)
            {
                return new ResponseModel { Status = false, Message = "Artwork not found" };
            }

            if (!artwork.IsDeleted)
            {
                return new ResponseModel { Status = false, Message = "Artwork is not deleted" };
            }

            artwork.IsDeleted = false;
            artwork.DeletionDate = null;
            artwork.DeletedBy = null;

            _unitOfWork.ArtworkRepository.Update(artwork);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Artwork restored successfully" };
        }

        public async Task<ResponseModel> UpdateArtworkAsync(Guid id, ArtworkUpdateModel model)
        {
            var artwork = await _unitOfWork.ArtworkRepository.GetAsync(id);
            if (artwork == null)
                return new ResponseModel { Status = false, Message = "Artwork not found" };

            _mapper.Map(model, artwork);
            _unitOfWork.ArtworkRepository.Update(artwork);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Artwork updated successfully" };
        }
    }
}
