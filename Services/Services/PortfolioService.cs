//using AutoMapper;
//using Repositories.Entities;
//using Repositories.Interfaces;
//using Repositories.Models.PortfolioModels;
//using Services.Common;
//using Services.Interfaces;
//using Services.Models.PortfolioModels;
//using Services.Models.ResponseModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Services.Services
//{
//    public class PortfolioService : IPortfolioService
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;

//        public PortfolioService(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }
//        //public async Task<ResponseModel> CreatePortfolioAsync(PortfolioCreateModel model)
//        //{
//        //    if (model == null || string.IsNullOrEmpty(model.Title))
//        //    {
//        //        return new ResponseModel { Status = false, Message = "Invalid input data" };
//        //    }

//        //    // ✅ Tạo mới Portfolio với ID trước khi map
//        //    var portfolio = new Portfolio
//        //    {
//        //        Id = Guid.NewGuid(), // 🔥 Đảm bảo ID được gán trước khi thêm vào DB
//        //        Title = model.Title,
//        //        Description = model.Description,
//        //        CoverImageUrl = model.CoverImageUrl,
//        //        UserId = model.UserId,
//        //        Skills = model.Skills,
//        //        Experience = model.Experience,
//        //        ContactUrl = model.ContactUrl,
//        //        PortfolioImages = new List<PortfolioImage>()
//        //    };

//        //    if (model.Images != null && model.Images.Any())
//        //    {
//        //        var artworkImageIds = model.Images.Select(img => img.ArtworkImageId).ToList();

//        //        // ✅ Kiểm tra tất cả ArtworkImageId có hợp lệ không
//        //        var existingArtworkImagesResult = await _unitOfWork.ArtworkImageRepository
//        //            .GetAllAsync(a => artworkImageIds.Contains(a.Id));
//        //        var existingArtworkImages = existingArtworkImagesResult.Data;

//        //        if (existingArtworkImages?.Count() != artworkImageIds.Count)
//        //        {
//        //            return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
//        //        }

//        //        // ✅ Thêm danh sách PortfolioImages vào Portfolio
//        //        foreach (var img in model.Images)
//        //        {
//        //            portfolio.PortfolioImages.Add(new PortfolioImage
//        //            {
//        //                PortfolioId = portfolio.Id, // 🔥 Gán ID của Portfolio
//        //                ArtworkImageId = img.ArtworkImageId,
//        //                ImageUrl = img.ImageUrl
//        //            });
//        //        }
//        //    }

//        //    await _unitOfWork.PortfolioRepository.AddAsync(portfolio);
//        //    Console.WriteLine($"Portfolio ID: {portfolio.Id}");
//        //    Console.WriteLine($"PortfolioImages count: {portfolio.PortfolioImages.Count}");

//        //    foreach (var img in portfolio.PortfolioImages)
//        //    {
//        //        Console.WriteLine($"ImageUrl: {img.ImageUrl}, ArtworkImageId: {img.ArtworkImageId}");
//        //    }

//        //    await _unitOfWork.SaveChangeAsync();

//        //    return new ResponseModel { Status = true, Message = "Portfolio created successfully" };
//        //}

//        public async Task<ResponseModel> CreatePortfolioAsync(PortfolioCreateModel model)
//        {
//            if (model == null || string.IsNullOrEmpty(model.Title))
//            {
//                return new ResponseModel { Status = false, Message = "Invalid input data" };
//            }

//            var portfolio = _mapper.Map<Portfolio>(model);

//            // 🌟 BƯỚC 1: Lưu Portfolio trước để lấy Id
//            await _unitOfWork.PortfolioRepository.AddAsync(portfolio);
//            await _unitOfWork.SaveChangeAsync(); // Đảm bảo Portfolio.Id đã có

//            if (model.Images != null && model.Images.Any())
//            {
//                var artworkImageIds = model.Images.Select(img => img.ArtworkImageId).ToList();

//                // ✅ Kiểm tra danh sách ArtworkImageId có hợp lệ không
//                var existingArtworkImagesResult = await _unitOfWork.ArtworkImageRepository
//                    .GetAllAsync(a => artworkImageIds.Contains(a.Id));
//                var existingArtworkImages = existingArtworkImagesResult.Data;

//                if (existingArtworkImages?.Count() != artworkImageIds.Count)
//                {
//                    return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
//                }

//                // 🌟 Kiểm tra xem PortfolioImage đã tồn tại chưa trước khi thêm
//                var existingPortfolioImagesResult = await _unitOfWork.PortfolioImageRepository
//                    .GetAllAsync(pi => pi.PortfolioId == portfolio.Id);
//                var existingPortfolioImages = existingPortfolioImagesResult.Data;

//                var newPortfolioImages = model.Images
//                    .Where(img => !existingPortfolioImages.Any(epi => epi.ArtworkImageId == img.ArtworkImageId))
//                    .Select(img => new PortfolioImage
//                    {
//                        PortfolioId = portfolio.Id,
//                        ArtworkImageId = img.ArtworkImageId,
//                        ImageUrl = img.ImageUrl
//                    }).ToList();

//                if (newPortfolioImages.Any())
//                {
//                    await _unitOfWork.PortfolioImageRepository.AddRangeAsync(newPortfolioImages);
//                    await _unitOfWork.SaveChangeAsync();
//                }
//            }

//            return new ResponseModel { Status = true, Message = "Portfolio created successfully" };
//        }






//        public async Task<ResponseModel> UpdatePortfolioAsync(Guid id, PortfolioUpdateModel model)
//        {
//            var portfolio = await _unitOfWork.PortfolioRepository.GetPortfolioByIdWithDetailsAsync(id);
//            if (portfolio == null)
//            {
//                return new ResponseModel { Status = false, Message = "Portfolio not found" };
//            }

//            _mapper.Map(model, portfolio);

//            if (model.Images != null && model.Images.Any())
//            {
//                var artworkImageIds = model.Images.Select(img => img.ArtworkImageId).ToList();
//                var existingArtworkImages = await _unitOfWork.ArtworkImageRepository.GetAllAsync(a => artworkImageIds.Contains(a.Id));

//                if (existingArtworkImages?.Count != artworkImageIds.Count)
//                {
//                    return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
//                }

//                portfolio.PortfolioImages = model.Images.Select(img => new PortfolioImage
//                {
//                    PortfolioId = portfolio.Id,
//                    ArtworkImageId = img.ArtworkImageId
//                }).ToList();
//            }

//            _unitOfWork.PortfolioRepository.Update(portfolio);
//            await _unitOfWork.SaveChangeAsync();

//            return new ResponseModel { Status = true, Message = "Portfolio updated successfully" };
//        }

//        public async Task<ResponseModel> DeletePortfolioAsync(Guid id)
//        {
//            var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(p => p.Id == id);
//            if (portfolio == null) return new ResponseModel { Status = false, Message = "Portfolio not found" };

//            _unitOfWork.PortfolioRepository.SoftDelete(portfolio);
//            await _unitOfWork.SaveChangeAsync();

//            return new ResponseModel { Status = true, Message = "Portfolio deleted successfully" };
//        }

//        public async Task<ResponseDataModel<PortfolioModel>> GetPortfolioByIdAsync(Guid id)
//        {
//            var portfolio = await _unitOfWork.PortfolioRepository.GetPortfolioByIdWithDetailsAsync(id);
//            if (portfolio == null)
//            {
//                return new ResponseDataModel<PortfolioModel> { Status = false, Message = "Portfolio not found" };
//            }

//            var portfolioModel = _mapper.Map<PortfolioModel>(portfolio);
//            portfolioModel.ImageUrls = portfolio.PortfolioImages.Select(pi => pi.ArtworkImage.FileUrl).ToList();

//            return new ResponseDataModel<PortfolioModel> { Status = true, Data = portfolioModel };
//        }

//        public async Task<Pagination<PortfolioModel>> GetAllPortfolioAsync(PortfolioFilterModel filterModel)
//        {
//            var queryResult = await _unitOfWork.PortfolioRepository.GetAllWithDetailsAsync(
//                p => p.IsDeleted == filterModel.isDelete,
//                filterModel.PageIndex,
//                filterModel.PageSize
//            );

//            var portfolios = _mapper.Map<List<PortfolioModel>>(queryResult.Data);
//            foreach (var portfolio in portfolios)
//            {
//                portfolio.ImageUrls = queryResult.Data.FirstOrDefault(p => p.Id == portfolio.Id)?.PortfolioImages
//                    .Select(pi => pi.ArtworkImage.FileUrl)
//                    .ToList();
//            }

//            return new Pagination<PortfolioModel>(portfolios, filterModel.PageIndex, filterModel.PageSize, queryResult.TotalCount);
//        }


//        public async Task<ResponseModel> RestorePortfolio(Guid id)
//        {
//            var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(id);
//            if (portfolio == null)
//            {
//                return new ResponseModel { Status = false, Message = "Portfolio not found" };
//            }

//            if (!portfolio.IsDeleted)
//            {
//                return new ResponseModel { Status = false, Message = "Portfolio is not deleted" };
//            }

//            portfolio.IsDeleted = false;
//            portfolio.DeletionDate = null;
//            portfolio.DeletedBy = null;

//            _unitOfWork.PortfolioRepository.Update(portfolio);
//            await _unitOfWork.SaveChangeAsync();

//            return new ResponseModel { Status = true, Message = "Portfolio restored successfully" };
//        }

//    }

//}
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
        //public async Task<ResponseModel> CreatePortfolioAsync(PortfolioCreateModel model)
        //{
        //    if (model == null || string.IsNullOrEmpty(model.Title))
        //    {
        //        return new ResponseModel { Status = false, Message = "Invalid input data" };
        //    }

        //    var portfolio = _mapper.Map<Portfolio>(model);
        //    await _unitOfWork.PortfolioRepository.AddAsync(portfolio);
        //    await _unitOfWork.SaveChangeAsync();

        //    if (model.Images != null && model.Images.Any())
        //    {
        //        var artworkImageIds = model.Images.Select(img => img.ArtworkImageId).ToList();

        //        // Lấy danh sách ArtworkImage hợp lệ
        //        var existingArtworkImages = (await _unitOfWork.ArtworkImageRepository
        //            .GetAllAsync(a => artworkImageIds.Contains(a.Id), pageIndex: 1, pageSize: int.MaxValue))
        //            .Data;

        //        if (existingArtworkImages?.Count != artworkImageIds.Count)
        //        {
        //            return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
        //        }

        //        // Kiểm tra PortfolioImage đã tồn tại chưa
        //        var existingPortfolioImages = (await _unitOfWork.PortfolioImageRepository
        //            .GetAllAsync(p => p.PortfolioId == portfolio.Id && artworkImageIds.Contains(p.ArtworkImageId),
        //            pageIndex: 1, pageSize: int.MaxValue))
        //            .Data
        //            .Select(p => p.ArtworkImageId)
        //            .ToHashSet(); 

        //        var newPortfolioImages = model.Images
        //            .Where(img => !existingPortfolioImages.Contains(img.ArtworkImageId)) // Tránh trùng lặp
        //            .Select(img => new PortfolioImage
        //            {
        //                PortfolioId = portfolio.Id,
        //                ArtworkImageId = img.ArtworkImageId
        //            }).ToList();

        //        if (newPortfolioImages.Any())
        //        {
        //            await _unitOfWork.PortfolioImageRepository.AddRangeAsync(newPortfolioImages);
        //            await _unitOfWork.SaveChangeAsync();
        //        }
        //    }

        //    return new ResponseModel { Status = true, Message = "Portfolio created successfully" };
        //}
        public async Task<ResponseModel> CreatePortfolioAsync(PortfolioCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                return new ResponseModel { Status = false, Message = "Invalid input data" };
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
                    .Where(id => !existingPortfolioImages.Contains(id)) // Tránh trùng lặp
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

            return new ResponseModel { Status = true, Message = "Portfolio created successfully" };
        }



        public async Task<ResponseModel> UpdatePortfolioAsync(Guid id, PortfolioUpdateModel model)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetPortfolioByIdWithDetailsAsync(id);
            if (portfolio == null)
            {
                return new ResponseModel { Status = false, Message = "Portfolio not found" };
            }

            _mapper.Map(model, portfolio);

            if (model.Images != null && model.Images.Any())
            {
                var artworkImageIds = model.Images.Select(img => img.ArtworkImageId).ToList();
                //var existingArtworkImages = (await _unitOfWork.ArtworkImageRepository.GetAllAsync(a => artworkImageIds.Contains(a.Id))).Data;
                var existingArtworkImages = (await _unitOfWork.ArtworkImageRepository
    .GetAllAsync(a => artworkImageIds.Contains(a.Id), pageIndex: 1, pageSize: int.MaxValue))
    .Data;


                if (existingArtworkImages?.Count != artworkImageIds.Count)
                {
                    return new ResponseModel { Status = false, Message = "One or more ArtworkImageIds are invalid" };
                }

                portfolio.PortfolioImages = model.Images.Select(img => new PortfolioImage
                {
                    PortfolioId = portfolio.Id,
                    ArtworkImageId = img.ArtworkImageId
                }).ToList();
            }

            _unitOfWork.PortfolioRepository.Update(portfolio);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Portfolio updated successfully" };
        }

        public async Task<ResponseModel> DeletePortfolioAsync(Guid id)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(p => p.Id == id);
            if (portfolio == null) return new ResponseModel { Status = false, Message = "Portfolio not found" };

            _unitOfWork.PortfolioRepository.SoftDelete(portfolio);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Portfolio deleted successfully" };
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






        //public async Task<Pagination<PortfolioModel>> GetAllPortfolioAsync(PortfolioFilterModel filterModel)
        //{
        //    var queryResult = await _unitOfWork.PortfolioRepository.GetAllWithDetailsAsync(
        //        p => p.IsDeleted == filterModel.isDelete,
        //        filterModel.PageIndex,
        //        filterModel.PageSize
        //    );

        //    var portfolios = _mapper.Map<List<PortfolioModel>>(queryResult.Data);
        //    portfolios.ForEach(portfolio => portfolio.ImageUrls = queryResult.Data.FirstOrDefault(p => p.Id == portfolio.Id)?.PortfolioImages
        //        .Select(pi => pi.ArtworkImage.FileUrl).ToList());

        //    return new Pagination<PortfolioModel>(portfolios, filterModel.PageIndex, filterModel.PageSize, queryResult.TotalCount);
        //}

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
