using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Models.ArtworkImageModels;
using Repositories.Models.ArtworkModels;
using Services.Common;
using Services.Interfaces;
using Services.Models.ArtworkModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                artwork.ArtworkCategories = model.CategoryIds.Select(categoryId => new ArtworkCategory
                {
                    CategoryId = categoryId
                }).ToList();
            }

            if (model.ImageUrls != null && model.ImageUrls.Any())
            {
                artwork.Images = model.ImageUrls.Select(url => new ArtworkImage
                {
                    FileUrl = url,
                    CreatedBy = model.CreatorId.GetValueOrDefault(Guid.Empty)
                }).ToList();
            }

            await _unitOfWork.ArtworkRepository.AddAsync(artwork);
            await _unitOfWork.SaveChangeAsync();

            // Fetch artwork again to include related data
            //var createdArtwork = await _unitOfWork.ArtworkRepository
            //    .GetAsync(artwork.Id, includes: new[] { "ArtworkCategories", "Images" });
            var createdArtwork = await _unitOfWork.ArtworkRepository
    .GetAsync(artwork.Id, includes: new[] { "ArtworkCategories", "ArtworkCategories.Category", "Images" });
            var artworkModel = _mapper.Map<ArtworkModel>(createdArtwork);

            return new ResponseModel
            {
                Status = true,
                Message = "Artwork created successfully",
                Data = artworkModel
            };
        }

        public async Task<ResponseModel> UpdateArtworkAsync(Guid id, ArtworkUpdateModel model)
        {
            var artwork = await _unitOfWork.ArtworkRepository
                .GetAsync(id, includes: new[] { "ArtworkCategories", "Images" });

            if (artwork == null)
                return new ResponseModel { Status = false, Message = "Artwork not found" };

            _mapper.Map(model, artwork);
            _unitOfWork.ArtworkRepository.Update(artwork);
            await _unitOfWork.SaveChangeAsync();

            var oldCategories = await _unitOfWork.ArtworkCategoryRepository.GetAllAsync(ac => ac.ArtworkId == id);
            _unitOfWork.ArtworkCategoryRepository.HardDeleteRange(oldCategories.Data);
            await _unitOfWork.SaveChangeAsync();

            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                var newCategories = model.CategoryIds.Select(categoryId => new ArtworkCategory
                {
                    ArtworkId = artwork.Id,
                    CategoryId = categoryId
                }).ToList();

                await _unitOfWork.ArtworkCategoryRepository.AddRangeAsync(newCategories);
            }

            await _unitOfWork.SaveChangeAsync();

            var existingImages = await _unitOfWork.ArtworkImageRepository.GetAllAsync(ai => ai.ArtworkId == id);
            _unitOfWork.ArtworkImageRepository.HardDeleteRange(existingImages.Data);

            if (model.ImageUrls != null && model.ImageUrls.Any())
            {
                var newImages = model.ImageUrls.Select(url => new ArtworkImage
                {
                    ArtworkId = id,
                    FileUrl = url
                }).ToList();
                await _unitOfWork.ArtworkImageRepository.AddRangeAsync(newImages);
            }

            await _unitOfWork.SaveChangeAsync();

            var updatedArtwork = await _unitOfWork.ArtworkRepository
                .GetAsync(id, includes: new[] { "ArtworkCategories.Category", "Images" });

            var artworkModel = _mapper.Map<ArtworkModel>(updatedArtwork);

            return new ResponseModel
            {
                Status = true,
                Message = "Artwork updated successfully",
                Data = artworkModel
            };
        }





        public async Task<ResponseModel> DeleteArtworkAsync(Guid id)
        {
            var artwork = await _unitOfWork.ArtworkRepository
                .GetAsync(id, includes: new[] { "ArtworkCategories", "ArtworkCategories.Category", "Images" });

            if (artwork == null)
                return new ResponseModel { Status = false, Message = "Artwork not found" };

            foreach (var image in artwork.Images)
            {
                image.IsDeleted = true;
            }

            artwork.IsDeleted = true;
            await _unitOfWork.SaveChangeAsync();

            var artworkModel = _mapper.Map<ArtworkModel>(artwork);

            return new ResponseModel
            {
                Status = true,
                Message = "Artwork deleted successfully",
                Data = artworkModel
            };
        }

        public async Task<Pagination<ArtworkModel>> GetAllArtworkAsync(ArtworkFilterModel filterModel)
        {
            var queryResult = await _unitOfWork.ArtworkRepository.GetAllAsyncs(
                a => (a.IsDeleted == filterModel.isDeleted) &&
                     (filterModel.MinPrice == null || a.Price >= filterModel.MinPrice) &&
                     (filterModel.MaxPrice == null || a.Price <= filterModel.MaxPrice) &&
                     (filterModel.CategoryId == null || a.ArtworkCategories.Any(ac => ac.CategoryId == filterModel.CategoryId)) &&
                     (filterModel.CreatorId == null || a.CreatorId == filterModel.CreatorId) &&
                     (filterModel.Status == null || a.Status == filterModel.Status),
                filterModel.PageIndex,
                filterModel.PageSize,
                a => a.Images,
                a => a.ArtworkCategories
            );
            var artworks = _mapper.Map<List<ArtworkModel>>(queryResult.Data);

            return new Pagination<ArtworkModel>(artworks, filterModel.PageIndex, filterModel.PageSize, queryResult.TotalCount);
        }
        public async Task<ResponseDataModel<ArtworkModel>> GetArtworkByIdAsync(Guid id)
        {
            var artwork = await _unitOfWork.ArtworkRepository.GetArtworkByIdWithDetailsAsync(id);

            if (artwork == null || artwork.IsDeleted)
            {
                return new ResponseDataModel<ArtworkModel> { Status = false, Message = "Artwork not found" };
            }

            artwork.ArtworkCategories ??= new List<ArtworkCategory>();
            artwork.Images ??= new List<ArtworkImage>();

            var artworkModel = _mapper.Map<ArtworkModel>(artwork);

            return new ResponseDataModel<ArtworkModel> { Status = true, Data = artworkModel };
        }
        
        public async Task<ResponseModel> RestoreArtwork(Guid id)
        {
            var artwork = await _unitOfWork.ArtworkRepository
                .GetAsync(id, includes: new[] { "ArtworkCategories", "ArtworkCategories.Category", "Images" });

            if (artwork == null)
                return new ResponseModel { Status = false, Message = "Artwork not found" };

            if (!artwork.IsDeleted)
                return new ResponseModel { Status = false, Message = "Artwork is not deleted" };

            artwork.IsDeleted = false;
            artwork.DeletionDate = null;
            artwork.DeletedBy = null;

            _unitOfWork.ArtworkRepository.Update(artwork);
            await _unitOfWork.SaveChangeAsync();

            var artworkModel = _mapper.Map<ArtworkModel>(artwork);

            return new ResponseModel
            {
                Status = true,
                Message = "Artwork restored successfully",
                Data = artworkModel
            };
        }



        public async Task<ResponseList<List<ArtworkImageModel>>> GetArtworkImagesByCreatorAsync(Guid creatorId)
        {
            var (imageList, totalCount) = await _unitOfWork.ArtworkImageRepository
       .GetAllAsync(img => img.CreatedBy == creatorId && !img.IsDeleted);
            if (!imageList.Any())
                return new ResponseList<List<ArtworkImageModel>>
                {
                    Status = false,
                    Message = "No images found",
                    Data = null
                };

            var imageModels = _mapper.Map<List<ArtworkImageModel>>(imageList);

            return new ResponseList<List<ArtworkImageModel>>
            {
                Status = true,
                Message = "Artwork images retrieved successfully",
                Data = imageModels
            };
        }
    }


}
