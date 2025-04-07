using AutoMapper;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Models.PortfolioModels;
using Services.Common;
using Services.Interfaces;
using Services.Models.PortfolioModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PortfolioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
       
        public async Task<ResponseModel> CreatePortfolioAsync(PortfolioCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                return new ResponseModel { Status = false, Message = "Title and image can't be null" };
            }

            var portfolio = _mapper.Map<Portfolio>(model);
            await _unitOfWork.PortfolioRepository.AddAsync(portfolio);
            await _unitOfWork.SaveChangeAsync();

            if (model.ArtworkImageIds != null && model.ArtworkImageIds.Any())
            {
                var artworkImageIds = model.ArtworkImageIds.ToList();

                // Lấy danh sách ArtworkImage hợp lệ
                var existingArtworkImages = (await _unitOfWork.ArtworkImageRepository
                    .GetAllAsync(a => artworkImageIds.Contains(a.Id), pageIndex: 1, pageSize: int.MaxValue))
                    .Data;

                if (existingArtworkImages?.Count != artworkImageIds.Count)
                {
                    return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
                }

                // Kiểm tra PortfolioImage đã tồn tại chưa
                var existingPortfolioImages = (await _unitOfWork.PortfolioImageRepository
                    .GetAllAsync(p => p.PortfolioId == portfolio.Id && artworkImageIds.Contains(p.ArtworkImageId),
                    pageIndex: 1, pageSize: int.MaxValue))
                    .Data
                    .Select(p => p.ArtworkImageId)
                    .ToHashSet();

                var newPortfolioImages = artworkImageIds
                    .Where(id => !existingPortfolioImages.Contains(id)) 
                    .Select(id => new PortfolioImage
                    {
                        PortfolioId = portfolio.Id,
                        ArtworkImageId = id
                    }).ToList();

                if (newPortfolioImages.Any())
                {
                    await _unitOfWork.PortfolioImageRepository.AddRangeAsync(newPortfolioImages);
                    await _unitOfWork.SaveChangeAsync();
                }
            }

            var portfolioModel = _mapper.Map<PortfolioModel>(portfolio); 
            portfolioModel.ImageUrls = portfolio.PortfolioImages.Select(pi => pi.ArtworkImage.FileUrl).ToList();
            return new ResponseModel
            {
                Status = true,
                Message = "Portfolio created successfully",
                Data = portfolioModel
            };
        }


        //public async Task<ResponseModel> UpdatePortfolioAsync(Guid id, PortfolioUpdateModel model)
        //{
        //    var portfolio = await _unitOfWork.PortfolioRepository.GetPortfolioByIdWithDetailsAsync(id);
        //    if (portfolio == null)
        //    {
        //        return new ResponseModel { Status = false, Message = "Portfolio not found" };
        //    }

        //    _mapper.Map(model, portfolio);

        //    if (model.Images != null && model.Images.Any())
        //    {
        //        var artworkImageIds = model.Images.Select(img => img.ArtworkImageId).ToList();
        //        var existingArtworkImages = (await _unitOfWork.ArtworkImageRepository
        //            .GetAllAsync(a => artworkImageIds.Contains(a.Id), pageIndex: 1, pageSize: int.MaxValue))
        //            .Data;

        //        if (existingArtworkImages?.Count != artworkImageIds.Count)
        //        {
        //            return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
        //        }

        //        portfolio.PortfolioImages = model.Images.Select(img => new PortfolioImage
        //        {
        //            PortfolioId = portfolio.Id,
        //            ArtworkImageId = img.ArtworkImageId
        //        }).ToList();
        //    }

        //    _unitOfWork.PortfolioRepository.Update(portfolio);
        //    await _unitOfWork.SaveChangeAsync();

        //    var portfolioModel = _mapper.Map<PortfolioModel>(portfolio); 
        //    portfolioModel.ImageUrls = portfolio.PortfolioImages.Select(pi => pi.ArtworkImage.FileUrl).ToList();
        //    return new ResponseModel
        //    {
        //        Status = true,
        //        Message = "Portfolio updated successfully",
        //        Data = portfolioModel
        //    };
        //}
        public async Task<ResponseModel> UpdatePortfolioAsync(Guid id, PortfolioUpdateModel model)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetPortfolioByIdWithDetailsAsync(id);
            if (portfolio == null)
            {
                return new ResponseModel { Status = false, Message = "Không tìm thấy portfolio" };
            }

            _mapper.Map(model, portfolio);

            if (model.ArtworkImageIds != null && model.ArtworkImageIds.Any())
            {
                var artworkImageIds = model.ArtworkImageIds.ToList();

                var existingArtworkImages = (await _unitOfWork.ArtworkImageRepository
                    .GetAllAsync(a => artworkImageIds.Contains(a.Id), pageIndex: 1, pageSize: int.MaxValue))
                    .Data;

                if (existingArtworkImages?.Count != artworkImageIds.Count)
                {
                    return new ResponseModel { Status = false, Message = "Một hoặc nhiều ArtworkImageId không hợp lệ" };
                }

                var existingPortfolioImageIds = portfolio.PortfolioImages
                    .Select(pi => pi.ArtworkImageId)
                    .ToHashSet();

                var imagesToRemove = portfolio.PortfolioImages
                    .Where(pi => !artworkImageIds.Contains(pi.ArtworkImageId))
                    .ToList();
                foreach (var image in imagesToRemove)
                {
                    portfolio.PortfolioImages.Remove(image);
                }

                var newPortfolioImages = artworkImageIds
                    .Where(id => !existingPortfolioImageIds.Contains(id))
                    .Select(id => new PortfolioImage
                    {
                        PortfolioId = portfolio.Id,
                        ArtworkImageId = id
                    }).ToList();

                foreach (var newImage in newPortfolioImages)
                {
                    portfolio.PortfolioImages.Add(newImage);
                }
            }

            _unitOfWork.PortfolioRepository.Update(portfolio);
            await _unitOfWork.SaveChangeAsync();

            var portfolioModel = _mapper.Map<PortfolioModel>(portfolio);
            portfolioModel.ImageUrls = portfolio.PortfolioImages.Select(pi => pi.ArtworkImage.FileUrl).ToList();
            return new ResponseModel
            {
                Status = true,
                Message = "Cập nhật portfolio thành công",
                Data = portfolioModel
            };
        }

        public async Task<ResponseModel> DeletePortfolioAsync(Guid id)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(p => p.Id == id);
            if (portfolio == null)
                return new ResponseModel { Status = false, Message = "Portfolio not found" };

            _unitOfWork.PortfolioRepository.SoftDelete(portfolio);
            await _unitOfWork.SaveChangeAsync();

            var portfolioModel = _mapper.Map<PortfolioModel>(portfolio); 
            return new ResponseModel
            {
                Status = true,
                Message = "Portfolio deleted successfully",
                Data = portfolioModel
            };
        }

        public async Task<ResponseDataModel<PortfolioModel>> GetPortfolioByIdAsync(Guid id)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetPortfolioByIdWithDetailsAsync(id);
            if (portfolio == null)
            {
                return new ResponseDataModel<PortfolioModel> { Status = false, Message = "Portfolio not found" };
            }

            var portfolioModel = _mapper.Map<PortfolioModel>(portfolio);
            portfolioModel.ImageUrls = portfolio.PortfolioImages.Select(pi => pi.ArtworkImage.FileUrl).ToList();

            return new ResponseDataModel<PortfolioModel> { Status = true, Data = portfolioModel };
        }


        public async Task<Pagination<PortfolioModel>> GetAllPortfolioAsync(PortfolioFilterModel filterModel)
        {
            var queryResult = await _unitOfWork.PortfolioRepository.GetAllWithDetailsAsync(
                p => p.IsDeleted == filterModel.isDelete,
                filterModel.PageIndex,
                filterModel.PageSize
            );

            // Ánh xạ danh sách Portfolio sang PortfolioDto
            var portfolios = _mapper.Map<List<PortfolioModel>>(queryResult.Data);

            return new Pagination<PortfolioModel>(portfolios, filterModel.PageIndex, filterModel.PageSize, queryResult.TotalCount);
        }

        public async Task<ResponseModel> RestorePortfolio(Guid id)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(id);
            if (portfolio == null)
            {
                return new ResponseModel { Status = false, Message = "Portfolio not found" };
            }

            if (!portfolio.IsDeleted)
            {
                return new ResponseModel { Status = false, Message = "Portfolio is not deleted" };
            }

            portfolio.IsDeleted = false;
            portfolio.DeletionDate = null;
            portfolio.DeletedBy = null;

            _unitOfWork.PortfolioRepository.Update(portfolio);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Portfolio restored successfully" };
        }
    }
}
