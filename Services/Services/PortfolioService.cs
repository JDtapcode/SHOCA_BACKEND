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
using System.Text;
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
        //public async Task<ResponseModel> CreatePortfolioAsync(PortfolioCreateModel model)
        //{
        //    if (model == null || string.IsNullOrEmpty(model.Title))
        //    {
        //        return new ResponseModel { Status = false, Message = "Invalid input data" };
        //    }

        //    // ✅ Tạo mới Portfolio với ID trước khi map
        //    var portfolio = new Portfolio
        //    {
        //        Id = Guid.NewGuid(), // 🔥 Đảm bảo ID được gán trước khi thêm vào DB
        //        Title = model.Title,
        //        Description = model.Description,
        //        CoverImageUrl = model.CoverImageUrl,
        //        UserId = model.UserId,
        //        Skills = model.Skills,
        //        Experience = model.Experience,
        //        ContactUrl = model.ContactUrl,
        //        PortfolioImages = new List<PortfolioImage>()
        //    };

        //    if (model.Images != null && model.Images.Any())
        //    {
        //        var artworkImageIds = model.Images.Select(img => img.ArtworkImageId).ToList();

        //        // ✅ Kiểm tra tất cả ArtworkImageId có hợp lệ không
        //        var existingArtworkImagesResult = await _unitOfWork.ArtworkImageRepository
        //            .GetAllAsync(a => artworkImageIds.Contains(a.Id));
        //        var existingArtworkImages = existingArtworkImagesResult.Data;

        //        if (existingArtworkImages?.Count() != artworkImageIds.Count)
        //        {
        //            return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
        //        }

        //        // ✅ Thêm danh sách PortfolioImages vào Portfolio
        //        foreach (var img in model.Images)
        //        {
        //            portfolio.PortfolioImages.Add(new PortfolioImage
        //            {
        //                PortfolioId = portfolio.Id, // 🔥 Gán ID của Portfolio
        //                ArtworkImageId = img.ArtworkImageId,
        //                ImageUrl = img.ImageUrl
        //            });
        //        }
        //    }

        //    await _unitOfWork.PortfolioRepository.AddAsync(portfolio);
        //    Console.WriteLine($"Portfolio ID: {portfolio.Id}");
        //    Console.WriteLine($"PortfolioImages count: {portfolio.PortfolioImages.Count}");

        //    foreach (var img in portfolio.PortfolioImages)
        //    {
        //        Console.WriteLine($"ImageUrl: {img.ImageUrl}, ArtworkImageId: {img.ArtworkImageId}");
        //    }

        //    await _unitOfWork.SaveChangeAsync();

        //    return new ResponseModel { Status = true, Message = "Portfolio created successfully" };
        //}

        public async Task<ResponseModel> CreatePortfolioAsync(PortfolioCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                return new ResponseModel { Status = false, Message = "Invalid input data" };
            }

            var portfolio = _mapper.Map<Portfolio>(model);

            // 🌟 BƯỚC 1: Lưu portfolio trước để tạo Id
            await _unitOfWork.PortfolioRepository.AddAsync(portfolio);
            await _unitOfWork.SaveChangeAsync();

            if (model.Images != null && model.Images.Any())
            {
                var artworkImageIds = model.Images.Select(img => img.ArtworkImageId).ToList();

                // ✅ Kiểm tra xem ArtworkImageId có hợp lệ không
                var existingArtworkImagesResult = await _unitOfWork.ArtworkImageRepository
                    .GetAllAsync(a => artworkImageIds.Contains(a.Id));
                var existingArtworkImages = existingArtworkImagesResult.Data;

                if (existingArtworkImages?.Count() != artworkImageIds.Count)
                {
                    return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
                }

                // 🌟 BƯỚC 2: Gán PortfolioId đúng sau khi Portfolio đã có Id
                var portfolioImages = model.Images.Select(img => new PortfolioImage
                {
                    PortfolioId = portfolio.Id,
                    ArtworkImageId = img.ArtworkImageId,
                    ImageUrl = img.ImageUrl
                }).ToList();

                await _unitOfWork.PortfolioImageRepository.AddRangeAsync(portfolioImages);
                await _unitOfWork.SaveChangeAsync();
            }

            return new ResponseModel { Status = true, Message = "Portfolio created successfully" };
        }



        public async Task<ResponseModel> UpdatePortfolioAsync(Guid id, PortfolioUpdateModel model)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(id);
            if (portfolio == null)
            {
                return new ResponseModel { Status = false, Message = "Portfolio not found" };
            }

            _mapper.Map(model, portfolio);

            if (model.Images != null && model.Images.Any())
            {
                var artworkImageIds = model.Images.Select(img => img.ArtworkImageId).ToList();

                // ✅ Kiểm tra xem tất cả ArtworkImageId có hợp lệ không
                var existingArtworkImagesResult = await _unitOfWork.ArtworkImageRepository
                    .GetAllAsync(a => artworkImageIds.Contains(a.Id));

                var existingArtworkImages = existingArtworkImagesResult.Data;

                if (existingArtworkImages?.Count() != artworkImageIds.Count)
                {
                    return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
                }

                // ✅ Chỉ xóa ảnh cũ nếu ảnh mới khác ảnh cũ
                var newImageUrls = model.Images.Select(img => img.ImageUrl).ToList();
                portfolio.PortfolioImages = portfolio.PortfolioImages
    .Where(img => newImageUrls.Contains(img.ImageUrl))
    .ToList();


                // ✅ Thêm ảnh mới nếu chưa có
                foreach (var img in model.Images)
                {
                    if (!portfolio.PortfolioImages.Any(p => p.ImageUrl == img.ImageUrl))
                    {
                        portfolio.PortfolioImages.Add(new PortfolioImage
                        {
                            PortfolioId = portfolio.Id, // 🔥 Đảm bảo có PortfolioId
                            ArtworkImageId = img.ArtworkImageId,
                            ImageUrl = img.ImageUrl
                        });
                    }
                }
            }

            _unitOfWork.PortfolioRepository.Update(portfolio);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Portfolio updated successfully" };
        }



        //public async Task<ResponseModel> UpdatePortfolioAsync(Guid id, PortfolioUpdateModel model)
        //{
        //    var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(id);
        //    if (portfolio == null)
        //        return new ResponseModel { Status = false, Message = "Portfolio not found" };

        //    _mapper.Map(model, portfolio);

        //    if (model.Images != null && model.Images.Any()) // ✅ Sửa model.ImageUrls thành model.Images
        //    {
        //        portfolio.PortfolioImages.Clear();

        //        var artworkImageIds = model.Images.Select(img => img.ArtworkImageId).ToList();

        //        // ✅ Sửa lỗi QueryResultModel
        //        var existingArtworkImagesResult = await _unitOfWork.ArtworkImageRepository
        //            .GetAllAsync(a => artworkImageIds.Contains(a.Id));

        //        var existingArtworkImages = existingArtworkImagesResult.Data; // 🔥 Sửa lại từ Items thành Data

        //        if (existingArtworkImages?.Count() != artworkImageIds.Count) // ✅ Kiểm tra null và sửa Count
        //        {
        //            return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
        //        }

        //        portfolio.PortfolioImages = model.Images.Select(img => new PortfolioImage
        //        {
        //            ArtworkImageId = img.ArtworkImageId,
        //            ImageUrl = img.ImageUrl
        //        }).ToList();
        //    }

        //    _unitOfWork.PortfolioRepository.Update(portfolio);
        //    await _unitOfWork.SaveChangeAsync();

        //    return new ResponseModel { Status = true, Message = "Portfolio updated successfully" };
        //}




        public async Task<ResponseModel> DeletePortfolioAsync(Guid id)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(id);
            if (portfolio == null) return new ResponseModel { Status = false, Message = "Portfolio not found" };

            _unitOfWork.PortfolioRepository.SoftDelete(portfolio);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Portfolio deleted successfully" };
        }

        public async Task<ResponseDataModel<PortfolioModel>> GetPortfolioByIdAsync(Guid id)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(id);
            if (portfolio == null || portfolio.IsDeleted)
            {
                return new ResponseDataModel<PortfolioModel> { Status = false, Message = "Portfolio not found" };
            }

            var portfolioModel = _mapper.Map<PortfolioModel>(portfolio);
            return new ResponseDataModel<PortfolioModel> { Status = true, Data = portfolioModel };
        }

        public async Task<Pagination<PortfolioModel>> GetAllPortfolioAsync(PortfolioFilterModel filterModel)
        {
            var queryResult = await _unitOfWork.PortfolioRepository.GetAllAsync(
                filter: p => (p.IsDeleted == filterModel.isDelete) &&
                             (string.IsNullOrEmpty(filterModel.Skills) || p.Skills.Contains(filterModel.Skills)) &&
                             (string.IsNullOrEmpty(filterModel.Experience) || p.Experience.Contains(filterModel.Experience)),
                pageIndex: filterModel.PageIndex,
                pageSize: filterModel.PageSize
            );

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
