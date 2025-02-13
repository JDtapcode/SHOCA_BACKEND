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

        public async Task<ResponseModel> CreatePortfolioAsync(PortfolioCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                return new ResponseModel { Status = false, Message = "Invalid input data" };
            }

            var portfolio = _mapper.Map<Portfolio>(model);
            await _unitOfWork.PortfolioRepository.AddAsync(portfolio);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Portfolio created successfully" };
        }

        public async Task<ResponseModel> DeletePortfolioAsync(Guid id)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(id);
            if (portfolio == null) return new ResponseModel { Status = false, Message = "Portfolio not found" };

            _unitOfWork.PortfolioRepository.SoftDelete(portfolio);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Portfolio deleted successfully" };
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

        public async Task<ResponseModel> UpdatePortfolioAsync(Guid id, PortfolioUpdateModel model)
        {
            var portfolio = await _unitOfWork.PortfolioRepository.GetAsync(id);
            if (portfolio == null)
                return new ResponseModel { Status = false, Message = "Portfolio not found" };

            _mapper.Map(model, portfolio);
            _unitOfWork.PortfolioRepository.Update(portfolio);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Portfolio updated successfully" };
        }
    }
}
