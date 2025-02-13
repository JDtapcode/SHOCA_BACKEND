using AutoMapper;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Models.ProPackages;
using Services.Common;
using Services.Interfaces;
using Services.Models.ProPackageModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProPackageService : IProPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProPackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateProPackageAsync(ProPackageCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                return new ResponseModel { Status = false, Message = "Invalid input data" };
            }

            var proPackage = _mapper.Map<ProPackage>(model);
            await _unitOfWork.ProPackageRepository.AddAsync(proPackage);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "ProPackage created successfully" };
        }

        public async Task<ResponseModel> DeleteProPackageAsync(Guid id)
        {
            var proPackage = await _unitOfWork.ProPackageRepository.GetAsync(id);
            if (proPackage == null) return new ResponseModel { Status = false, Message = "ProPackage not found" };

            _unitOfWork.ProPackageRepository.SoftDelete(proPackage);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "ProPackage deleted successfully" };
        }

        public async Task<Pagination<ProPackageModel>> GetAllProPackageAsync(ProPackageFilterModel filterModel)
        {
            var queryResult = await _unitOfWork.ProPackageRepository.GetAllAsync(
                filter: p => p.IsDeleted == filterModel.isDeleted,
                pageIndex: filterModel.PageIndex,
                pageSize: filterModel.PageSize
            );

            var proPackages = _mapper.Map<List<ProPackageModel>>(queryResult.Data);
            return new Pagination<ProPackageModel>(proPackages, filterModel.PageIndex, filterModel.PageSize, queryResult.TotalCount);
        }

        public async Task<ResponseDataModel<ProPackageModel>> GetProPackageByIdAsync(Guid id)
        {
            var proPackage = await _unitOfWork.ProPackageRepository.GetAsync(id);
            if (proPackage == null || proPackage.IsDeleted)
            {
                return new ResponseDataModel<ProPackageModel> { Status = false, Message = "ProPackage not found" };
            }

            var proPackageModel = _mapper.Map<ProPackageModel>(proPackage);
            return new ResponseDataModel<ProPackageModel> { Status = true, Data = proPackageModel };
        }

        public async Task<ResponseModel> RestoreProPackage(Guid id)
        {
            var proPackage = await _unitOfWork.ProPackageRepository.GetAsync(id);
            if (proPackage == null)
            {
                return new ResponseModel { Status = false, Message = "ProPackage not found" };
            }

            if (!proPackage.IsDeleted)
            {
                return new ResponseModel { Status = false, Message = "ProPackage is not deleted" };
            }

            proPackage.IsDeleted = false;
            proPackage.DeletionDate = null;
            proPackage.DeletedBy = null;

            _unitOfWork.ProPackageRepository.Update(proPackage);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "ProPackage restored successfully" };
        }

        public async Task<ResponseModel> UpdateProPackageAsync(Guid id, ProPackageUpdateModel model)
        {
            var proPackage = await _unitOfWork.ProPackageRepository.GetAsync(id);
            if (proPackage == null)
                return new ResponseModel { Status = false, Message = "ProPackage not found" };

            _mapper.Map(model, proPackage);
            _unitOfWork.ProPackageRepository.Update(proPackage);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "ProPackage updated successfully" };
        }
    }

}
