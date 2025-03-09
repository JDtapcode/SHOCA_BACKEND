using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
using System.Linq.Expressions;
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

            var proPackage = new ProPackage
            {
                Name = model.Name,
                Price = model.Price,
                Duration = model.Duration,
                Features = model.Features.Select(f => new ProPackageFeature { Name = f }).ToList()
            };

            await _unitOfWork.ProPackageRepository.AddAsync(proPackage);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "ProPackage created successfully" };
        }

        public async Task<ResponseDataModel<ProPackageModel>> GetProPackageByIdAsync(Guid id)
        {
            var proPackage = await _unitOfWork.ProPackageRepository.GetProPackageByIdWithDetailsAsync(id);

            if (proPackage == null)
            {
                return new ResponseDataModel<ProPackageModel>
                {
                    Status = false,
                    Message = "ProPackage not found"
                };
            }

            var proPackageModel = _mapper.Map<ProPackageModel>(proPackage);
            proPackageModel.Features = proPackage.Features.Select(f => f.Name).ToList(); 

            return new ResponseDataModel<ProPackageModel>
            {
                Status = true,
                Data = proPackageModel
            };
        }


        public async Task<ResponseModel> DeleteProPackageAsync(Guid id)
        {
            var proPackage = await _unitOfWork.ProPackageRepository.GetAsync(id);

            if (proPackage == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "ProPackage not found"
                };
            }

            if (proPackage.IsDeleted)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "ProPackage is already deleted"
                };
            }

            _unitOfWork.ProPackageRepository.SoftDelete(proPackage);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Status = true,
                Message = "ProPackage deleted successfully"
            };
        }


        public async Task<Pagination<ProPackageModel>> GetAllProPackageAsync(ProPackageFilterModel filterModel)
        {
            var queryResult = await _unitOfWork.ProPackageRepository.GetAllAsync(
                filter: p => p.IsDeleted == filterModel.isDeleted,
                pageIndex: filterModel.PageIndex,
                pageSize: filterModel.PageSize
            );

            // Đảm bảo Data không bị null trước khi mapping
            var proPackages = _mapper.Map<List<ProPackageModel>>(queryResult.Data ?? new List<ProPackage>());

            return new Pagination<ProPackageModel>(
                proPackages,
                filterModel.PageIndex,
                filterModel.PageSize,
                queryResult.TotalCount
            );
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
            var proPackage = await _unitOfWork.ProPackageRepository
                .GetProPackageByIdWithDetailsAsync(id); 

            if (proPackage == null)
            {
                return new ResponseModel { Status = false, Message = "ProPackage not found" };
            }

            proPackage.Name = model.Name ?? proPackage.Name;
            proPackage.Price = model.Price;
            proPackage.Duration = model.Duration ?? proPackage.Duration;

            if (model.Features != null)
            {
                var existingFeatureNames = proPackage.Features.Select(f => f.Name).ToHashSet(); 
                var newFeatures = model.Features.Where(f => !existingFeatureNames.Contains(f)) 
                                                .Select(f => new ProPackageFeature { Name = f })
                                                .ToList();

                foreach (var feature in newFeatures)
                {
                    proPackage.Features.Add(feature);
                }
            }

            _unitOfWork.ProPackageRepository.Update(proPackage);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "ProPackage updated successfully" };
        }



        
    }

}
